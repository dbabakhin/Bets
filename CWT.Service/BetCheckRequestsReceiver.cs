using CWT.Domain.Interfaces;
using Shared.Common.Interfaces;
using Shared.Common.Messages;

namespace CWT.Service
{
    public class BetCheckRequestsReceiver : BackgroundService
    {
        private readonly ILogger<BetCheckRequestsReceiver> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMessageConsumer<string, BetConfirmRequestMessage> _consumer;

        public BetCheckRequestsReceiver(ILogger<BetCheckRequestsReceiver> logger, IServiceProvider serviceProvider, IMessageConsumer<string, BetConfirmRequestMessage> consumer)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _consumer = consumer ?? throw new ArgumentNullException(nameof(consumer));
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken) => Task.Run(() => ConsumeAsync(stoppingToken));

        private async Task ConsumeAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            while (!stoppingToken.IsCancellationRequested)
            {
                var consumeResult = _consumer.Consume(stoppingToken);
                if (consumeResult.Value != null)
                {
                    using (IServiceScope scope = _serviceProvider.CreateScope())
                    {
                        var cwt = scope.ServiceProvider.GetRequiredService<ICWTService>();
                        _ = await cwt.CheckEnoughtMoneyAsync(consumeResult.Value, stoppingToken);
                    }
                    _consumer.Commit();
                }
            }
            _consumer.Close();
        }
    }
}