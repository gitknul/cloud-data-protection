using System;
using System.Threading.Tasks;
using AutoMapper;
using CloudDataProtection.Core.Controllers;
using CloudDataProtection.Core.Jwt;
using CloudDataProtection.Core.Messaging;
using CloudDataProtection.Core.Messaging.Dto;
using CloudDataProtection.Core.Rest.Errors;
using CloudDataProtection.Core.Result;
using CloudDataProtection.Services.Subscription.Business;
using CloudDataProtection.Services.Subscription.Controllers.Dto.Input;
using CloudDataProtection.Services.Subscription.Controllers.Dto.Output;
using CloudDataProtection.Services.Subscription.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CloudDataProtection.Services.Subscription.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Policy = "ClientOnly")]
    public class BackupConfigurationController : ServiceController
    {
        private readonly Lazy<BackupConfigurationBusinessLogic> _logic;
        private readonly Lazy<BackupSchemeBusinessLogic> _schemeLogic;
        private readonly Lazy<IMessagePublisher<BackupConfigurationEnteredMessage>> _messagePublisher;
        private readonly IMapper _mapper;

        public BackupConfigurationController(IJwtDecoder jwtDecoder, Lazy<BackupConfigurationBusinessLogic> logic, Lazy<BackupSchemeBusinessLogic> schemeLogic, IMapper mapper, Lazy<IMessagePublisher<BackupConfigurationEnteredMessage>> messagePublisher) : base(jwtDecoder)
        {
            _logic = logic;
            _schemeLogic = schemeLogic;
            _mapper = mapper;
            _messagePublisher = messagePublisher;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult> Get()
        {
            if (UserRole != Core.Controllers.Data.UserRole.Client)
            {
                return Unauthorized();
            }
            
            BusinessResult<BackupConfiguration> businessResult = await _logic.Value.GetByUser(UserId);
            
            if (!businessResult.Success)
            {
                return Problem("An error occured while attempting to retrieve the backup configuration");
            }

            BackupConfigurationOutput output = _mapper.Map<BackupConfigurationOutput>(businessResult.Data);

            return Ok(output);
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult> Create(CreateBackupConfigurationInput input)
        {
            if (UserRole != Core.Controllers.Data.UserRole.Client)
            {
                return Unauthorized();
            }

            BusinessResult<BackupScheme> schemeResult = await _schemeLogic.Value.Get(input.BackupSchemeId);

            if (!schemeResult.Success)
            {
                return Problem("An error occured while attempting to create the backup configuration");
            }

            BackupConfiguration configuration = _mapper.Map<BackupConfiguration>(schemeResult.Data);

            configuration.UserId = UserId;

            BusinessResult<BackupConfiguration> businessResult = await _logic.Value.Create(configuration);

            if (!businessResult.Success)
            {
                if (businessResult.Data == null)
                {
                    return Problem("An error occured while attempting to create the backup configuration");
                }

                return Conflict(ConflictResponse.Create(businessResult.Message));
            }

            BackupConfigurationEnteredMessage message = new BackupConfigurationEnteredMessage
            {
                UserId = businessResult.Data.UserId
            };

            await _messagePublisher.Value.Send(message);
            
            CreateBackupConfigurationResult result = _mapper.Map<CreateBackupConfigurationResult>(businessResult.Data);

            return Ok(result);
        }
    }
}