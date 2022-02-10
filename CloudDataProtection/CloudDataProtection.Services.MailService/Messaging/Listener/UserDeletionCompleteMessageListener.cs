using System.Threading.Tasks;
using CloudDataProtection.Core.Messaging;
using CloudDataProtection.Core.Messaging.RabbitMq;
using CloudDataProtection.Services.MailService.Business;
using CloudDataProtection.Services.MailService.Dto;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CloudDataProtection.Services.MailService.Messaging.Listener
{
    public class UserDeletionCompleteMessageListener : RabbitMqMessageListener<UserDeletionCompleteModel>
    {
        private readonly AccountMailLogic _logic;

        public UserDeletionCompleteMessageListener(IOptions<RabbitMqConfiguration> options, ILogger<UserDeletionCompleteMessageListener> logger, AccountMailLogic logic) : base(options, logger)
        {
            _logic = logic;
        }

        protected override string RoutingKey => RoutingKeys.UserDeletionComplete;
        public override async Task HandleMessage(UserDeletionCompleteModel model)
        {
            await _logic.SendUserDeletionComplete(model.Email);
        }
    }
}