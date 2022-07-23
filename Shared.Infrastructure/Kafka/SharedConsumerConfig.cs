using Confluent.Kafka;
using Shared.Common.Interfaces;

namespace Shared.Infrastructure.Kafka
{
    public class SharedConsumerConfig
    {
        public string TopicName { get; set; }
        public string BootstrapServers { get; set; }
        public string GroupId { get; set; }

        public IConsumer<string, string> Build()
        {

            var cfg = new ConsumerConfig()
            {
                BootstrapServers = BootstrapServers,
                GroupId = GroupId,
                EnableAutoCommit = false,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            var b = new ConsumerBuilder<string, string>(cfg);

            var c = b.Build();
            c.Subscribe(TopicName);

            return c;
        }
    }
}
