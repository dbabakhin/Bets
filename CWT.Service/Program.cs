using Bets.Infrastructure.Kafka;
using CWT.Domain.Interfaces;
using CWT.Domain.Services;
using CWT.Infrastructure;
using CWT.Service;
using Shared.Common.Messages;
using Shared.Infrastructure.Kafka;

const string CONSUMER_SECTION = "Kafka:Consumer";
const string PRODUCER_SECTION = "Kafka:Producer";

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((ctx, services) =>
    {

        var cfgConsumer = new SharedConsumerConfig<BetConfirmRequestMessage>();
        ctx.Configuration.GetSection(CONSUMER_SECTION).Bind(cfgConsumer);

        services.AddSingleton(a =>
        {
            return cfgConsumer.Build();
        });

        var cfgProducer = new SharedProducerConfig<BetConfirmResultMessage>();
        ctx.Configuration.GetSection(PRODUCER_SECTION).Bind(cfgProducer);
        services.AddSingleton(a =>
        {
            return cfgProducer.Build();
        });

        services.AddScoped<ICWTRepository, CWTRepository>();
        services.AddScoped<ICWTService, CWTService>();

        services.AddHostedService<BetCheckRequestsReceiver>();
    })
    .Build();

await host.RunAsync();

