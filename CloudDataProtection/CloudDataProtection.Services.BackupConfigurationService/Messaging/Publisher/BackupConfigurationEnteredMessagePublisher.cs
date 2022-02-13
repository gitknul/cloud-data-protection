using CloudDataProtection.Core.Messaging;
using CloudDataProtection.Core.Messaging.Dto;
using CloudDataProtection.Core.Messaging.RabbitMq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CloudDataProtection.Services.Subscription.Messaging.Publisher
{
    public class BackupConfigurationEnteredMessagePublisher : RabbitMqMessagePublisher<BackupConfigurationEnteredMessage>
    {
        public BackupConfigurationEnteredMessagePublisher(IOptions<RabbitMqConfiguration> options, ILogger<BackupConfigurationEnteredMessagePublisher> logger) : base(options, logger)
        {
        }

        protected override string RoutingKey => RoutingKeys.BackupConfigurationEntered;
    }
}