using System.Threading.Tasks;
using CloudDataProtection.Core.Messaging;
using CloudDataProtection.Core.Messaging.Dto;
using CloudDataProtection.Core.Messaging.RabbitMq;
using CloudDataProtection.Services.MailService.Business;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CloudDataProtection.Services.MailService.Messaging.Listener
{
    public class GoogleAccountConnectedMessageListener : RabbitMqMessageListener<GoogleAccountConnectedMessage>
    {
        private readonly AccountMailLogic _logic;

        public GoogleAccountConnectedMessageListener(IOptions<RabbitMqConfiguration> options, ILogger<GoogleAccountConnectedMessageListener> logger, AccountMailLogic logic) : base(options, logger)
        {
            _logic = logic;
        }

        protected override string RoutingKey => RoutingKeys.GoogleAccountConnected;
        
        public override async Task HandleMessage(GoogleAccountConnectedMessage message)
        {
            await _logic.SendAccountConnected(message.Email);
        }
    }
}