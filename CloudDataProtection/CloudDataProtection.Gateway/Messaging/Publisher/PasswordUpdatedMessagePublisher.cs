using CloudDataProtection.Core.Messaging;
using CloudDataProtection.Core.Messaging.RabbitMq;
using CloudDataProtection.Dto.Result;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CloudDataProtection.Messaging.Publisher
{
    public class PasswordUpdatedMessagePublisher : RabbitMqMessagePublisher<PasswordUpdatedModel>
    {
        public PasswordUpdatedMessagePublisher(IOptions<RabbitMqConfiguration> options, ILogger<PasswordUpdatedMessagePublisher> logger) : base(options, logger)
        {
        }

        protected override string RoutingKey => RoutingKeys.PasswordUpdated;
    }
}