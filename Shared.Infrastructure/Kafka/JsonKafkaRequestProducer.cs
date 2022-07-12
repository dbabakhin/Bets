using Confluent.Kafka;
using Shared.Common.Interfaces;
using System.Text.Json;

namespace Bets.Infrastructure.Kafka
{
    public class JsonKafkaProducer<TValue> : IMessageProducer<string, TValue>
    {
        private readonly string _topicName;
        private readonly IProducer<string, string> _kafkaProducer;

        public JsonKafkaProducer(string topicName, IProducer<string, string> kafkaProducer)
        {
            _topicName = topicName ?? throw new ArgumentNullException(nameof(topicName));
            _kafkaProducer = kafkaProducer ?? throw new ArgumentNullException(nameof(kafkaProducer));
        }

        public async Task ProduceAsync(string key, TValue message, CancellationToken ct)
        {
            var json = JsonSerializer.Serialize(message);
            _ = await _kafkaProducer.ProduceAsync(_topicName, new Message<string, string> { Key = null, Value = json }, ct);
        }
    }

}
