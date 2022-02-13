using System.Threading.Tasks;
using CloudDataProtection.Core.Messaging;
using CloudDataProtection.Core.Messaging.Dto;
using CloudDataProtection.Services.MailService.Business;
using CloudDataProtection.Core.Messaging.RabbitMq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CloudDataProtection.Services.MailService.Messaging.Listener
{
    public class ClientRegisteredMessageListener : RabbitMqMessageListener<ClientRegisteredMessage>
    {
        private readonly RegistrationMailLogic _logic;

        public ClientRegisteredMessageListener(IOptions<RabbitMqConfiguration> options, ILogger<ClientRegisteredMessageListener> logger, RegistrationMailLogic logic) : base(options, logger)
        {
            _logic = logic;
        }

        protected override string RoutingKey => RoutingKeys.ClientRegistered;

        public override async Task HandleMessage(ClientRegisteredMessage message)
        {
            await _logic.SendClientRegistered(message.Email);
        }
    }
}