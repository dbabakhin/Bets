namespace Shared.Common.Interfaces
{
    public interface IMessageProducer<TKey, TMessage>
    {
        Task ProduceAsync(TKey key, TMessage message, CancellationToken ct);
    }
}
