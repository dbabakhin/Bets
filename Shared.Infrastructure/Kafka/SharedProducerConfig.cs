using Confluent.Kafka;
using Shared.Common.Interfaces;

namespace Bets.Infrastructure.Kafka
{
    public class SharedProducerConfig
    {
        public string TopicName { get; set; }
        public string BootstrapServers { get; set; }

        public IProducer<string, string> Build()
        {
            var producerBuilder = new ProducerBuilder<string, string>(new ProducerConfig()
            {
                BootstrapServers = BootstrapServers
            });
            var producer = producerBuilder.Build();

            return producer;
        }
    }

}
