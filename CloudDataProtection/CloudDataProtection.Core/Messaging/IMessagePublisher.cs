using System.Threading.Tasks;

namespace CloudDataProtection.Core.Messaging
{
    public interface IMessagePublisher<in TMessage>
    {
        Task Send(TMessage model);
    }
}