using Bets.Infrastructure.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Common.Interfaces;
using Shared.Infrastructure.Kafka;

namespace Shared.Infrastructure.DI
{
    public static class KafkaExtensions
    {
        const string CONSUMER_SECTION = "Kafka:Consumer";
        const string PRODUCER_SECTION = "Kafka:Producer";

        public static IServiceCollection AddKafkaConsumer<TValue>(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<SharedConsumerConfig>().Bind(configuration.GetSection(CONSUMER_SECTION)).Validate(a =>
            {
                return !string.IsNullOrWhiteSpace(a.TopicName)
                    && !string.IsNullOrWhiteSpace(a.BootstrapServers)
                    && !string.IsNullOrWhiteSpace(a.GroupId);
            }, "All consumer field required");
            services.AddSingleton<IMessageConsumer<string, TValue>, KafkaJsonConsumer<TValue>>();
            return services;
        }

        public static IServiceCollection AddKafkaProducer<TValue>(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<SharedProducerConfig>().Bind(configuration.GetSection(PRODUCER_SECTION)).Validate(a =>
            {
                return !string.IsNullOrWhiteSpace(a.BootstrapServers) && !string.IsNullOrWhiteSpace(a.TopicName);
            }, "All producer fields required");

            services.AddSingleton<IMessageProducer<string, TValue>, JsonKafkaProducer<TValue>>();
            return services;
        }
    }
}
