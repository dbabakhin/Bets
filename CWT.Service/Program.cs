using CWT.Domain.Interfaces;
using CWT.Domain.Services;
using CWT.Infrastructure;
using CWT.Service;
using Shared.Common.Messages;
using Shared.Infrastructure.DI;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((ctx, services) =>
    {
        services.AddKafkaConsumer<BetConfirmRequestMessage>(ctx.Configuration);
        services.AddKafkaProducer<BetConfirmResultMessage>(ctx.Configuration);

        services.AddScoped<ICWTRepository, CWTRepository>();
        services.AddScoped<ICWTService, CWTService>();

        services.AddHostedService<BetCheckRequestsReceiver>();
    })
    .Build();

await host.RunAsync();

