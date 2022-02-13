using System;
using System.Threading.Tasks;
using CloudDataProtection.Business;
using CloudDataProtection.Controllers.Dto.Input;
using CloudDataProtection.Controllers.Dto.Output;
using CloudDataProtection.Core.Messaging;
using CloudDataProtection.Core.Messaging.Dto;
using CloudDataProtection.Core.Rest.Errors;
using CloudDataProtection.Core.Result;
using CloudDataProtection.Entities;
using CloudDataProtection.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CloudDataProtection.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly AuthenticationBusinessLogic _logic;
        private readonly IJwtHelper _jwtHelper;
        private readonly Lazy<IMessagePublisher<ClientRegisteredMessage>> _messagePublisher;

        public AuthenticationController(AuthenticationBusinessLogic logic, IJwtHelper jwtHelper, 
            Lazy<IMessagePublisher<ClientRegisteredMessage>> messagePublisher)
        {
            _logic = logic;
            _jwtHelper = jwtHelper;
            _messagePublisher = messagePublisher;
        }
        
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<ActionResult> Authenticate([FromBody] AuthenticateInput input)
        {
            BusinessResult<User> businessResult = await _logic.Authenticate(input.Email, input.Password);

            if (!businessResult.Success)
            {
                return Unauthorized(UnauthorizedResponse.Create());
            }

            User user = businessResult.Data;

            AuthenticateOutput output = new AuthenticateOutput
            {
                User = new LoginUserOutput
                {
                    Email = user.Email,
                    Id = user.Id,
                    Role = user.Role
                },
                Token = _jwtHelper.GenerateToken(user)
            };
            
            return Ok(output);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterInput model)
        {
            User user = new User
            {
                Email = model.Email,
                Role = UserRole.Client
            };

            BusinessResult<User> businessResult = await _logic.Create(user, model.Password);

            if (!businessResult.Success)
            {
                return Conflict(ConflictResponse.Create(businessResult.Message));
            }
            
            ClientRegisteredMessage registeredMessage = new ClientRegisteredMessage
            {
                Email = user.Email,
                Id = user.Id
            };

            await _messagePublisher.Value.Send(registeredMessage);
            
            return Ok(registeredMessage);
        }
    }
}