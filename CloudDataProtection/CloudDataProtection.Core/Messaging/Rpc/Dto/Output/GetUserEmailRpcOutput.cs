namespace CloudDataProtection.Core.Messaging.Rpc.Dto.Output
{
    public class GetUserEmailRpcOutput
    {
        public string Email { get; set; }
        public RpcResponseStatus Status { get; set; }
        public string StatusMessage { get; set; }
    }
}