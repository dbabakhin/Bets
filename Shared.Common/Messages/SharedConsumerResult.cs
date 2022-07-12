namespace Shared.Common.Messages
{
    public class SharedConsumerResult<TKey, TValue>
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }
    }
}
