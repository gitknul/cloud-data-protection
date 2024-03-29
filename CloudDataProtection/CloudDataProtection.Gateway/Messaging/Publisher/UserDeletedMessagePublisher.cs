﻿using CloudDataProtection.Core.Messaging;
using CloudDataProtection.Core.Messaging.RabbitMq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CloudDataProtection.Messaging.Publisher
{
    public class UserDeletedMessagePublisher : RabbitMqMessagePublisher<UserDeletedMessage>
    {
        public UserDeletedMessagePublisher(IOptions<RabbitMqConfiguration> options, ILogger<UserDeletedMessagePublisher> logger) : base(options, logger)
        {
        }

        protected override string RoutingKey => RoutingKeys.ClientDeleted;
    }
    
    public class UserDeletedMessage
    {
        public long UserId { get; set; }
        
        public string Email { get; set; }
    }
}