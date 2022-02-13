using System.Threading.Tasks;
using CloudDataProtection.Core.Messaging;
using CloudDataProtection.Core.Messaging.Dto;
using CloudDataProtection.Core.Messaging.RabbitMq;
using CloudDataProtection.Services.MailService.Business;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CloudDataProtection.Services.MailService.Messaging.Listener
{
    public class EmailChangeRequestedMessageListener : RabbitMqMessageListener<EmailChangeRequestedMessage>
    {
        private readonly AccountMailLogic _logic;

        public EmailChangeRequestedMessageListener(IOptions<RabbitMqConfiguration> options, ILogger<EmailChangeRequestedMessageListener> logger, AccountMailLogic logic) : base(options, logger)
        {
            _logic = logic;
        }

        protected override string RoutingKey => RoutingKeys.EmailChangeRequested;
        
        public override async Task HandleMessage(EmailChangeRequestedMessage message)
        {
            await _logic.SendEmailChangeRequested(message.NewEmail, message.Url, message.ExpiresAt);
        }
    }
}