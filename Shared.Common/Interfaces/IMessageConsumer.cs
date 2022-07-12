using Shared.Common.Messages;

namespace Shared.Common.Interfaces
{
    public interface IMessageConsumer <TKey, TValue>
    {
        SharedConsumerResult<TKey, TValue> Consume(CancellationToken ct);
        void Commit();
        void Close();
    }    
}
