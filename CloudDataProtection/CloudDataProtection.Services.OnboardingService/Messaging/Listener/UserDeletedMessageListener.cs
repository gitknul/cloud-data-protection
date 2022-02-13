using System;
using System.Threading.Tasks;
using CloudDataProtection.Core.Messaging;
using CloudDataProtection.Core.Messaging.Dto;
using CloudDataProtection.Core.Messaging.RabbitMq;
using CloudDataProtection.Services.Onboarding.Business;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CloudDataProtection.Services.Onboarding.Messaging.Listener
{
    public class UserDeletedMessageListener : RabbitMqMessageListener<UserDeletedMessage>
    {
        private readonly IServiceScope _scope;

        public UserDeletedMessageListener(IOptions<RabbitMqConfiguration> options, ILogger<UserDeletedMessageListener> logger, IServiceScopeFactory serviceScopeFactory) : base(options, logger)
        {
            _scope = serviceScopeFactory.CreateScope();
        }

        protected override string RoutingKey => RoutingKeys.ClientDeleted;
        
        public override async Task HandleMessage(UserDeletedMessage message)
        {
            DateTime start = DateTime.Now;
            
            OnboardingBusinessLogic logic =
                _scope.ServiceProvider.GetRequiredService<OnboardingBusinessLogic>();
            
            await logic.DeleteByUser(message.UserId);

            IMessagePublisher<UserDataDeletedMessage> publisher =
                _scope.ServiceProvider.GetRequiredService<IMessagePublisher<UserDataDeletedMessage>>();

            UserDataDeletedMessage dataDeletedMessage = new UserDataDeletedMessage
            {
                UserId = message.UserId,
                StartedAt = start,
                CompletedAt = DateTime.Now
            };

            await publisher.Send(dataDeletedMessage);
        }
    }
}