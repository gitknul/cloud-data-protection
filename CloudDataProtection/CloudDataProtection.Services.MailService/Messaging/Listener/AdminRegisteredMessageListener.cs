using System.Threading.Tasks;
using CloudDataProtection.Core.Messaging;
using CloudDataProtection.Core.Messaging.Dto;
using CloudDataProtection.Core.Messaging.RabbitMq;
using CloudDataProtection.Services.MailService.Business;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CloudDataProtection.Services.MailService.Messaging.Listener
{
    public class AdminRegisteredMessageListener : RabbitMqMessageListener<AdminRegisteredMessage>
    {
        private readonly RegistrationMailLogic _registrationMailLogic;

        public AdminRegisteredMessageListener(IOptions<RabbitMqConfiguration> options, ILogger<AdminRegisteredMessageListener> logger, RegistrationMailLogic registrationMailLogic) : base(options, logger)
        {
            _registrationMailLogic = registrationMailLogic;
        }

        protected override string RoutingKey => RoutingKeys.AdminRegistered;
        
        public override async Task HandleMessage(AdminRegisteredMessage message)
        {
            ResetPasswordMessage resetPasswordMessage = new ResetPasswordMessage
            {
                Id = message.Id,
                Email = message.Email,
                Expiration = message.Expiration,
                Url = message.Url
            };
            
            await _registrationMailLogic.SendAdminRegistered(resetPasswordMessage);
        }
    }
}