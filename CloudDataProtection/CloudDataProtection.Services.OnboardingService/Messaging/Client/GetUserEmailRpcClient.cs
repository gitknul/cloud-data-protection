using CloudDataProtection.Core.Messaging.RabbitMq;
using CloudDataProtection.Core.Messaging.Rpc.Dto.Input;
using CloudDataProtection.Core.Messaging.Rpc.Dto.Output;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CloudDataProtection.Services.Onboarding.Messaging.Client
{
    public class GetUserEmailRpcClient : RabbitMqRpcClient<GetUserEmailRpcInput, GetUserEmailRpcOutput>
    {
        public GetUserEmailRpcClient(IOptions<RabbitMqConfiguration> options, ILogger<GetUserEmailRpcClient> logger) : base(options, logger)
        {
        }
    }
}