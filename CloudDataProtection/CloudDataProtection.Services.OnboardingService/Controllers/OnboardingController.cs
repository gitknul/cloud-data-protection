using System;
using System.Threading.Tasks;
using AutoMapper;
using CloudDataProtection.Core.Controllers;
using CloudDataProtection.Core.Jwt;
using CloudDataProtection.Core.Messaging;
using CloudDataProtection.Core.Messaging.Dto;
using CloudDataProtection.Core.Messaging.Rpc.Dto.Input;
using CloudDataProtection.Core.Messaging.Rpc.Dto.Output;
using CloudDataProtection.Core.Rest.Errors;
using CloudDataProtection.Core.Result;
using CloudDataProtection.Services.Onboarding.Business;
using CloudDataProtection.Services.Onboarding.Config;
using CloudDataProtection.Services.Onboarding.Controllers.Dto.Output;
using CloudDataProtection.Services.Onboarding.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CloudDataProtection.Services.Onboarding.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Policy = "ClientOnly")]
    public class OnboardingController : ServiceController
    {
        private readonly Lazy<OnboardingBusinessLogic> _logic;
        private readonly IMapper _mapper;
        private readonly OnboardingOptions _options;
        private readonly Lazy<IMessagePublisher<GoogleAccountConnectedMessage>> _messagePublisher;
        private readonly Lazy<IRpcClient<GetUserEmailRpcInput, GetUserEmailRpcOutput>> _getUserEmailClient;

        public OnboardingController(
            Lazy<OnboardingBusinessLogic> logic, 
            IJwtDecoder jwtDecoder, 
            IMapper mapper, 
            IOptions<OnboardingOptions> options, 
            Lazy<IMessagePublisher<GoogleAccountConnectedMessage>> messagePublisher, 
            Lazy<IRpcClient<GetUserEmailRpcInput, GetUserEmailRpcOutput>> getUserEmailClient) : base(jwtDecoder)
        {
            _logic = logic;
            _mapper = mapper;
            _messagePublisher = messagePublisher;
            _getUserEmailClient = getUserEmailClient;
            _options = options.Value;
        }
        
        [HttpGet]
        [Route("")]
        public async Task<ActionResult> Get()
        {
            BusinessResult<Entities.Onboarding> businessResult = await _logic.Value.GetByUser(UserId);

            if (!businessResult.Success)
            {
                return NotFound(NotFoundResponse.Create("User", UserId));
            }

            OnboardingOutput output = _mapper.Map<OnboardingOutput>(businessResult.Data);

            if (businessResult.Data.Status == OnboardingStatus.None)
            {
                BusinessResult<GoogleLoginInfo> loginInfoBusinessResult = await _logic.Value.GetLoginInfo(UserId);

                if (!loginInfoBusinessResult.Success)
                {
                    return Problem(loginInfoBusinessResult.Message);
                }
                
                output.LoginInfo = _mapper.Map<GoogleLoginInfoOutput>(loginInfoBusinessResult.Data);
            }

            return Ok(output);
        }

        [HttpGet]
        [Route("GoogleLogin")]
        public async Task<ActionResult> GoogleLogin()
        {
            string code = Request.Query["code"];
            string token = Request.Query["state"];

            if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(token))
            {
                return BadRequest();
            }
            
            BusinessResult<GoogleCredentials> businessResult = await _logic.Value.CreateCredentials(code, token);

            if (!businessResult.Success)
            {
                string url = string.Concat(_options.RedirectUri, "?message=", businessResult.Message);
                
                return Redirect(url);
            }

            long userId = businessResult.Data.UserId;

            GetUserEmailRpcInput input = new GetUserEmailRpcInput(userId);

            GetUserEmailRpcOutput response = await _getUserEmailClient.Value.Request(input);

            GoogleAccountConnectedMessage message = new GoogleAccountConnectedMessage
            {
                UserId = userId,
                Email = response.Email
            };

            await _messagePublisher.Value.Send(message);
            
            return Redirect(_options.RedirectUri);
        }
    }
}