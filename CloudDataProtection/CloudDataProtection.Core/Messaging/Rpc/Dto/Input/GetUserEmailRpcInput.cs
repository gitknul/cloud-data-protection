namespace CloudDataProtection.Core.Messaging.Rpc.Dto.Input
{
    public class GetUserEmailRpcInput
    {
        public long UserId { get; }

        public GetUserEmailRpcInput(long userId)
        {
            UserId = userId;
        }
    }
}