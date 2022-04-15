using System;
using System.Threading.Tasks;
using CloudDataProtection.Business;
using CloudDataProtection.Core.Messaging;
using CloudDataProtection.Core.Messaging.Dto;
using CloudDataProtection.Core.Messaging.RabbitMq;
using CloudDataProtection.Core.Result;
using CloudDataProtection.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CloudDataProtection.Messaging.Listener
{
    public class UserDataDeletedMessageListener : RabbitMqMessageListener<UserDataDeletedMessage>
    {
        private readonly IServiceScope _scope;
        
        public UserDataDeletedMessageListener(IOptions<RabbitMqConfiguration> options, ILogger<UserDataDeletedMessageListener> logger, IServiceScopeFactory serviceScopeFactory) : base(options, logger)
        {
            _scope = serviceScopeFactory.CreateScope();
        }

        protected override string RoutingKey => RoutingKeys.ClientDataDeleted;

        public override async Task HandleMessage(UserDataDeletedMessage message)
        {
            UserBusinessLogic logic = _scope.ServiceProvider.GetRequiredService<UserBusinessLogic>();

            UserDeletionHistoryProgress history = new UserDeletionHistoryProgress
            {
                ServiceName = message.Service,
                StartedAt = message.StartedAt,
                CompletedAt = message.CompletedAt
            };

            BusinessResult<Tuple<UserDeletionHistory, string>> addProgressResult = await logic.AddProgress(history, message.UserId);

            if (addProgressResult.Success && addProgressResult.Data != null && addProgressResult.Data.Item1.IsComplete)
            {
                UserDeletionHistory progress = addProgressResult.Data.Item1;
                string email = addProgressResult.Data.Item2;
                
                IMessagePublisher<UserDeletionCompleteMessage> publisher =
                    _scope.ServiceProvider.GetRequiredService<IMessagePublisher<UserDeletionCompleteMessage>>();

                UserDeletionCompleteMessage deletionCompleteMessage = new UserDeletionCompleteMessage
                {
                    UserId = progress.UserId,
                    Email = email
                };

                await publisher.Send(deletionCompleteMessage);
            }
        }
    }
}