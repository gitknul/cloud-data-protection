using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CloudDataProtection.Business;
using CloudDataProtection.Business.Options;
using CloudDataProtection.Core.Messaging;
using CloudDataProtection.Core.Messaging.Dto;
using CloudDataProtection.Core.Result;
using CloudDataProtection.Entities;
using Microsoft.Extensions.Options;

namespace CloudDataProtection.Seeder
{
    public class AdminSeeder
    {
        private readonly AuthenticationBusinessLogic _logic;
        private readonly IMessagePublisher<AdminRegisteredMessage> _messagePublisher;
        private readonly AdminSeederOptions _options;
        private readonly ResetPasswordOptions _resetPasswordOptions;

        public AdminSeeder(AuthenticationBusinessLogic logic, IMessagePublisher<AdminRegisteredMessage> publisher, IOptions<AdminSeederOptions> options, IOptions<ResetPasswordOptions> resetPasswordOptions)
        {
            _logic = logic;
            _messagePublisher = publisher;
            _options = options.Value;
            _resetPasswordOptions = resetPasswordOptions.Value;
        }

        public async Task Seed()
        {
            BusinessResult<ICollection<User>> getResult = await _logic.GetAll(UserRole.Admin);

            if (!getResult.Success)
            {
                throw new Exception("An error occurred while attempting to seed the admin user");
            }

            if (getResult.Data.Count > 0)
            {
                return;
            }

            User adminUser = new User
            {
                Email = _options.Email,
                Role = UserRole.Admin
            };

            BusinessResult<User> createResult = await _logic.Create(adminUser);
            BusinessResult<ResetPasswordRequest> requestResetResult = await _logic.RequestResetPassword(adminUser);

            if (!createResult.Success || !requestResetResult.Success)
            {
                return;
            }

            AdminRegisteredMessage registeredMessage = new AdminRegisteredMessage
            {
                Email = adminUser.Email,
                Id = adminUser.Id,
                Url = _resetPasswordOptions.FormatUrl(requestResetResult.Data.Token),
                Expiration = requestResetResult.Data.ExpiresAt
            };

            await _messagePublisher.Send(registeredMessage);
        }
    }
}