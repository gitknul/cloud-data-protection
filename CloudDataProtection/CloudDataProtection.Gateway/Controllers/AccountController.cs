using System;
using System.Threading.Tasks;
using CloudDataProtection.Business;
using CloudDataProtection.Business.Options;
using CloudDataProtection.Controllers.Dto.Input;
using CloudDataProtection.Controllers.Dto.Output;
using CloudDataProtection.Core.Controllers;
using CloudDataProtection.Core.Jwt;
using CloudDataProtection.Core.Messaging;
using CloudDataProtection.Core.Messaging.Dto;
using CloudDataProtection.Core.Rest.Errors;
using CloudDataProtection.Core.Result;
using CloudDataProtection.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using EmailChangeRequestedMessage = CloudDataProtection.Messaging.Publisher.EmailChangeRequestedMessage;
using UserDeletedMessage = CloudDataProtection.Messaging.Publisher.UserDeletedMessage;

namespace CloudDataProtection.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class AccountController : ServiceController
    {
        private readonly Lazy<IMessagePublisher<UserDeletedMessage>> _userDeletedMessagePublisher;
        private readonly Lazy<IMessagePublisher<EmailChangeRequestedMessage>> _emailChangeRequestedMessagePublisher;
        private readonly Lazy<IMessagePublisher<PasswordUpdatedMessage>> _passwordUpdatedMessagePublisher;
        private readonly ChangeEmailOptions _changeEmailOptions;
        private readonly UserBusinessLogic _userBusinessLogic;
        private readonly AuthenticationBusinessLogic _authenticationBusinessLogic;
        
        public AccountController(IJwtDecoder jwtDecoder,
            Lazy<IMessagePublisher<UserDeletedMessage>> userDeletedMessagePublisher, 
            Lazy<IMessagePublisher<EmailChangeRequestedMessage>> emailChangeRequestedMessagePublisher,
            Lazy<IMessagePublisher<PasswordUpdatedMessage>> passwordUpdatedMessagePublisher,
            IOptions<ChangeEmailOptions> changeEmailOptions,
            UserBusinessLogic userBusinessLogic, 
            AuthenticationBusinessLogic authenticationBusinessLogic) : base(jwtDecoder)
        {
            _userDeletedMessagePublisher = userDeletedMessagePublisher;
            _userBusinessLogic = userBusinessLogic;
            _passwordUpdatedMessagePublisher = passwordUpdatedMessagePublisher;
            _changeEmailOptions = changeEmailOptions.Value;
            _authenticationBusinessLogic = authenticationBusinessLogic;
            _emailChangeRequestedMessagePublisher = emailChangeRequestedMessagePublisher;
        }

        [HttpPatch]
        [Route("Email")]
        public async Task<ActionResult> ChangeEmail(ChangeEmailInput input)
        {
            BusinessResult<ChangeEmailRequest> changeEmailResult = await _authenticationBusinessLogic.RequestChangeEmail(UserId, input.Email);

            if (!changeEmailResult.Success)
            {
                return Conflict(ConflictResponse.Create(changeEmailResult.Message));
            }

            ChangeEmailRequest changeEmailRequest = changeEmailResult.Data;

            EmailChangeRequestedMessage message = new EmailChangeRequestedMessage
            {
                NewEmail = changeEmailRequest.NewEmail,
                Url = _changeEmailOptions.FormatUrl(changeEmailRequest.Token),
                ExpiresAt = changeEmailRequest.ExpiresAt
            };

            await _emailChangeRequestedMessagePublisher.Value.Send(message);
            
            return Ok();
        }

        [HttpPatch]
        [Route("ConfirmEmail")]
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmChangeEmail(ConfirmChangeEmailInput input)
        {
            BusinessResult<string> changeEmailResult = await _authenticationBusinessLogic.ConfirmChangeEmail(input.Token);
            
            if (!changeEmailResult.Success)
            {
                return Conflict(ConflictResponse.Create(changeEmailResult.Message));
            }

            ConfirmChangeEmailOutput output = new ConfirmChangeEmailOutput
            {
                Email = changeEmailResult.Data
            };

            return Ok(output);
        }

        [HttpPatch]
        [Route("ChangePassword")]
        public async Task<ActionResult> ChangePassword(ChangePasswordInput input)
        {
            BusinessResult<User> changePasswordResult = await _authenticationBusinessLogic.ChangePassword(UserId, input.CurrentPassword, input.NewPassword);
            
            if (!changePasswordResult.Success)
            {
                return Conflict(ConflictResponse.Create(changePasswordResult.Message));
            }

            PasswordUpdatedMessage message = new PasswordUpdatedMessage
            {
                Email = changePasswordResult.Data.Email,
                UserId = changePasswordResult.Data.Id
            };

            await _passwordUpdatedMessagePublisher.Value.Send(message);

            return Ok();
        }
        
        [HttpPatch]
        [Route("ResetPassword")]
        [AllowAnonymous]
        public async Task<ActionResult> ResetPassword(ResetPasswordInput input)
        {
            BusinessResult<User> updatePasswordResult = await _authenticationBusinessLogic.UpdatePassword(input.Password, input.Token);
            
            if (!updatePasswordResult.Success)
            {
                return Conflict(ConflictResponse.Create(updatePasswordResult.Message));
            }

            PasswordUpdatedMessage message = new PasswordUpdatedMessage
            {
                UserId = updatePasswordResult.Data.Id,
                Email = updatePasswordResult.Data.Email
            };

            await _passwordUpdatedMessagePublisher.Value.Send(message);

            return Ok();
        }


        [HttpDelete]
        [Route("")]
        [Authorize(Policy = "ClientOnly")]
        public async Task<ActionResult> Delete()
        {
            BusinessResult<User> getUserResult = await _userBusinessLogic.Get(UserId);

            if (!getUserResult.Success || getUserResult.Data == null)
            {
                return Forbid();
            }

            User user = getUserResult.Data;

            UserDeletedMessage message = new UserDeletedMessage
            {
                Email = user.Email,
                UserId = user.Id
            };

            await _userBusinessLogic.Delete(user.Id);

            await _userDeletedMessagePublisher.Value.Send(message);
            
            return Accepted();
        }
    }
}