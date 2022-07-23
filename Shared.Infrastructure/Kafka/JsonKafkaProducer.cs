using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Shared.Common.Interfaces;
using System.Text.Json;

namespace Bets.Infrastructure.Kafka
{
    public class JsonKafkaProducer<TValue> : IMessageProducer<string, TValue>
    {
        private readonly string _topicName;
        private readonly IProducer<string, string> _kafkaProducer;

        public JsonKafkaProducer(IOptions<SharedProducerConfig> options)
        {
            _topicName = options.Value.TopicName ?? throw new ArgumentNullException(nameof(options));
            _kafkaProducer = options.Value.Build() ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task ProduceAsync(string key, TValue message, CancellationToken ct)
        {
            var json = JsonSerializer.Serialize(message);
            _ = await _kafkaProducer.ProduceAsync(_topicName, new Message<string, string> { Key = null, Value = json }, ct);
        }
    }

}
