using CloudDataProtection.Core.Messaging;
using CloudDataProtection.Core.Messaging.Dto;
using CloudDataProtection.Core.Messaging.RabbitMq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CloudDataProtection.Services.Subscription.Messaging.Publisher
{
    public class UserDataDeletedMessagePublisher : RabbitMqMessagePublisher<UserDataDeletedMessage>
    {
        public UserDataDeletedMessagePublisher(IOptions<RabbitMqConfiguration> options, ILogger<UserDataDeletedMessagePublisher> logger) : base(options, logger)
        {
        }

        protected override string RoutingKey => RoutingKeys.ClientDataDeleted;
    }
}