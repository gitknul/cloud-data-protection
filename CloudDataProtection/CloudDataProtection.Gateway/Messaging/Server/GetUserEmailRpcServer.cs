using System.Threading.Tasks;
using CloudDataProtection.Business;
using CloudDataProtection.Core.Messaging.RabbitMq;
using CloudDataProtection.Core.Messaging.Rpc;
using CloudDataProtection.Core.Messaging.Rpc.Dto.Input;
using CloudDataProtection.Core.Messaging.Rpc.Dto.Output;
using CloudDataProtection.Core.Result;
using CloudDataProtection.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CloudDataProtection.Messaging.Server
{
    public class GetUserEmailRpcServer : RabbitMqRpcServer<GetUserEmailRpcInput, GetUserEmailRpcOutput>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public GetUserEmailRpcServer(IOptions<RabbitMqConfiguration> options, ILogger<GetUserEmailRpcServer> logger, IServiceScopeFactory serviceScopeFactory) : base(options, logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public override async Task<GetUserEmailRpcOutput> HandleMessage(GetUserEmailRpcInput model)
        {
            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                UserBusinessLogic logic = scope.ServiceProvider.GetRequiredService<UserBusinessLogic>();
                
                BusinessResult<User> result = await logic.Get(model.UserId);

                if (!result.Success)
                {
                    return new GetUserEmailRpcOutput
                    {
                        Status = RpcResponseStatus.Error,
                        StatusMessage = result.Message
                    };
                }
        
                return new GetUserEmailRpcOutput
                {
                    Email = result.Data.Email,
                    Status = RpcResponseStatus.Ok
                };
            }
        }
    }
}