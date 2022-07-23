using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Shared.Common.Interfaces;
using Shared.Common.Messages;
using System.Text.Json;

namespace Shared.Infrastructure.Kafka
{
    public class KafkaJsonConsumer<TValue> : IMessageConsumer<string, TValue>
    {
        private readonly IConsumer<string, string> _kafkaConsumer;

        public KafkaJsonConsumer(IOptions<SharedConsumerConfig> options)
        {
            _kafkaConsumer = options.Value.Build();
        }

        public void Close() => _kafkaConsumer.Close();

        public void Commit() => _ = _kafkaConsumer.Commit();

        public SharedConsumerResult<string, TValue> Consume(CancellationToken ct)
        {
            var r = _kafkaConsumer.Consume(ct);
            var messageObj = JsonSerializer.Deserialize<TValue>(r.Message.Value) ?? throw new InvalidDataException("Unexpected message format");

            return new SharedConsumerResult<string, TValue>()
            {
                Key = r.Message.Key,
                Value = messageObj
            };
        }
    }
}
