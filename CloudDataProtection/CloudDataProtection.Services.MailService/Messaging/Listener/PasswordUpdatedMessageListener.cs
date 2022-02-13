using System.Threading.Tasks;
using CloudDataProtection.Core.Messaging;
using CloudDataProtection.Core.Messaging.RabbitMq;
using CloudDataProtection.Services.MailService.Business;
using CloudDataProtection.Services.MailService.Dto;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CloudDataProtection.Services.MailService.Messaging.Listener
{
    public class PasswordUpdatedMessageListener : RabbitMqMessageListener<PasswordUpdatedModel>
    {
        private readonly AccountMailLogic _mailLogic;

        public PasswordUpdatedMessageListener(IOptions<RabbitMqConfiguration> options, ILogger<PasswordUpdatedMessageListener> logger, AccountMailLogic mailLogic) : base(options, logger)
        {
            _mailLogic = mailLogic;
        }

        protected override string RoutingKey => RoutingKeys.PasswordUpdated;
        
        public override async Task HandleMessage(PasswordUpdatedModel model)
        {
            await _mailLogic.SendPasswordUpdated(model);
        }
    }
}