using System;
using System.Threading.Tasks;
using CloudDataProtection.Core.Messaging;
using CloudDataProtection.Core.Messaging.Dto;
using CloudDataProtection.Core.Messaging.RabbitMq;
using CloudDataProtection.Core.Result;
using CloudDataProtection.Services.Onboarding.Business;
using CloudDataProtection.Services.Onboarding.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CloudDataProtection.Services.Onboarding.Messaging.Listener
{
    public class BackupConfigurationEnteredMessageListener : RabbitMqMessageListener<BackupConfigurationEnteredMessage>
    {
        private readonly IServiceScope _scope;

        public BackupConfigurationEnteredMessageListener(IOptions<RabbitMqConfiguration> options, ILogger<BackupConfigurationEnteredMessageListener> logger, IServiceProvider serviceProvider) : base(options, logger)
        {
            _scope = serviceProvider.CreateScope();
        }

        protected override string RoutingKey => RoutingKeys.BackupConfigurationEntered;
        public override async Task HandleMessage(BackupConfigurationEnteredMessage message)
        {
            OnboardingBusinessLogic logic = _scope.ServiceProvider.GetRequiredService<OnboardingBusinessLogic>();
            
            BusinessResult<Entities.Onboarding> result = await logic.GetByUser(message.UserId);

            if (result.Success)
            {
                result.Data.Status = OnboardingStatus.SchemeEntered;

                await logic.Update(result.Data);
            }
        }
    }
}