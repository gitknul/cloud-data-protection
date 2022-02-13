using System.Threading.Tasks;
using CloudDataProtection.Core.Messaging;
using CloudDataProtection.Core.Messaging.Dto;
using CloudDataProtection.Core.Messaging.RabbitMq;
using CloudDataProtection.Services.MailService.Business;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CloudDataProtection.Services.MailService.Messaging.Listener
{
    public class PasswordUpdatedMessageListener : RabbitMqMessageListener<PasswordUpdatedMessage>
    {
        private readonly AccountMailLogic _mailLogic;

        public PasswordUpdatedMessageListener(IOptions<RabbitMqConfiguration> options, ILogger<PasswordUpdatedMessageListener> logger, AccountMailLogic mailLogic) : base(options, logger)
        {
            _mailLogic = mailLogic;
        }

        protected override string RoutingKey => RoutingKeys.PasswordUpdated;
        
        public override async Task HandleMessage(PasswordUpdatedMessage message)
        {
            await _mailLogic.SendPasswordUpdated(message);
        }
    }
}