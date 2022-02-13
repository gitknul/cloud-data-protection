using System.Threading.Tasks;
using CloudDataProtection.Core.Messaging;
using CloudDataProtection.Core.Messaging.RabbitMq;
using CloudDataProtection.Services.Onboarding.Business;
using CloudDataProtection.Services.Onboarding.Dto;
using CloudDataProtection.Services.Onboarding.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CloudDataProtection.Services.Onboarding.Messaging.Listener
{
    public class ClientRegisteredMessageListener : RabbitMqMessageListener<UserRegisteredModel>
    {
        private readonly IServiceScope _scope;

        public ClientRegisteredMessageListener(IOptions<RabbitMqConfiguration> options, ILogger<ClientRegisteredMessageListener> logger, IServiceScopeFactory serviceScopeFactory) :
            base(options, logger)
        {
            _scope = serviceScopeFactory.CreateScope();
        }

        protected override string RoutingKey => RoutingKeys.ClientRegistered;

        public override async Task HandleMessage(UserRegisteredModel model)
        {
            OnboardingBusinessLogic logic = _scope.ServiceProvider.GetRequiredService<OnboardingBusinessLogic>();

            Entities.Onboarding onboarding = new Entities.Onboarding
            {
                Status = OnboardingStatus.None,
                UserId = model.Id
            };

            await logic.Create(onboarding);
        }
    }
}