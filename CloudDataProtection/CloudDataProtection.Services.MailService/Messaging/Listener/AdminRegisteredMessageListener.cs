using System.Threading.Tasks;
using CloudDataProtection.Core.Messaging;
using CloudDataProtection.Core.Messaging.RabbitMq;
using CloudDataProtection.Services.MailService.Business;
using CloudDataProtection.Services.MailService.Dto;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CloudDataProtection.Services.MailService.Messaging.Listener
{
    public class AdminRegisteredMessageListener : RabbitMqMessageListener<AdminRegisteredModel>
    {
        private readonly RegistrationMailLogic _registrationMailLogic;

        public AdminRegisteredMessageListener(IOptions<RabbitMqConfiguration> options, ILogger<AdminRegisteredMessageListener> logger, RegistrationMailLogic registrationMailLogic) : base(options, logger)
        {
            _registrationMailLogic = registrationMailLogic;
        }

        protected override string RoutingKey => RoutingKeys.AdminRegistered;
        
        public override async Task HandleMessage(AdminRegisteredModel model)
        {
            ResetPasswordModel resetPasswordModel = new ResetPasswordModel
            {
                Id = model.Id,
                Email = model.Email,
                Expiration = model.Expiration,
                Url = model.Url
            };
            
            await _registrationMailLogic.SendAdminRegistered(resetPasswordModel);
        }
    }
}