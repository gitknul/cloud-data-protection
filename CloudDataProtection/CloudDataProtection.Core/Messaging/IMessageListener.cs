using System.Threading.Tasks;

namespace CloudDataProtection.Core.Messaging
{
    public interface IMessageListener<in TMessage>
    {
        Task HandleMessage(TMessage model);
    }
}