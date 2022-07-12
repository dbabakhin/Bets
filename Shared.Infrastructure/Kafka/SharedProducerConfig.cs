using Confluent.Kafka;
using Shared.Common.Interfaces;

namespace Bets.Infrastructure.Kafka
{
    public class SharedProducerConfig<TValue>
    {
        public string TopicName { get; set; }
        public string BootstrapServers { get; set; }

        public IMessageProducer<string,TValue> Build()
        {
            var producerBuilder = new ProducerBuilder<string, string>(new ProducerConfig()
            {
                BootstrapServers = BootstrapServers
            });
            var producer = producerBuilder.Build();

            return new JsonKafkaProducer<TValue>(TopicName, producer);
        }
    }

}
