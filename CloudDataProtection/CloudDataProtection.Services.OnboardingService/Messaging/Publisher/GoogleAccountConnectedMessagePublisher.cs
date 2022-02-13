using CloudDataProtection.Core.Messaging;
using CloudDataProtection.Core.Messaging.Dto;
using CloudDataProtection.Core.Messaging.RabbitMq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CloudDataProtection.Services.Onboarding.Messaging.Publisher
{
    public class GoogleAccountConnectedMessagePublisher : RabbitMqMessagePublisher<GoogleAccountConnectedMessage>
    {
        public GoogleAccountConnectedMessagePublisher(IOptions<RabbitMqConfiguration> options, ILogger<GoogleAccountConnectedMessagePublisher> logger) : base(options, logger)
        {
        }

        protected override string RoutingKey => RoutingKeys.GoogleAccountConnected;
    }
}