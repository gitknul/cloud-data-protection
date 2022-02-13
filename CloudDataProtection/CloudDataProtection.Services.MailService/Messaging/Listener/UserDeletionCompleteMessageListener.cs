using System.Threading.Tasks;
using CloudDataProtection.Core.Messaging;
using CloudDataProtection.Core.Messaging.Dto;
using CloudDataProtection.Core.Messaging.RabbitMq;
using CloudDataProtection.Services.MailService.Business;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CloudDataProtection.Services.MailService.Messaging.Listener
{
    public class UserDeletionCompleteMessageListener : RabbitMqMessageListener<UserDeletionCompleteMessage>
    {
        private readonly AccountMailLogic _logic;

        public UserDeletionCompleteMessageListener(IOptions<RabbitMqConfiguration> options, ILogger<UserDeletionCompleteMessageListener> logger, AccountMailLogic logic) : base(options, logger)
        {
            _logic = logic;
        }

        protected override string RoutingKey => RoutingKeys.ClientDeletionComplete;
        
        public override async Task HandleMessage(UserDeletionCompleteMessage message)
        {
            await _logic.SendUserDeletionComplete(message.Email);
        }
    }
}