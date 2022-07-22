using Bets.API.App;
using Bets.Domain.Enums;
using Bets.Domain.Models;
using Shared.Common.Interfaces;
using Shared.Common.Messages;

namespace Bets.API.Service
{
    public class CWTResultReceiver : BackgroundService
    {
        private readonly ILogger<CWTResultReceiver> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMessageConsumer<string, BetConfirmResultMessage> _consumer;

        public CWTResultReceiver(ILogger<CWTResultReceiver> logger, IServiceProvider serviceProvider, IMessageConsumer<string, BetConfirmResultMessage> consumer)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _consumer = consumer ?? throw new ArgumentNullException(nameof(consumer));
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(() => ConsumeAsync(stoppingToken));
        }

        private async Task ConsumeAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            while (true)
            {
                stoppingToken.ThrowIfCancellationRequested();

                var consumeResult = _consumer.Consume(stoppingToken);
                if (consumeResult.Value != null)
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var processor = scope.ServiceProvider.GetRequiredService<BetsProcessor>();
                        await processor.ProcessBetStatusAsync(new UpdateBetStatusModel(consumeResult.Value.BetId, consumeResult.Value.Allowed), stoppingToken);
                    }
                    _consumer.Commit();
                }
            }
            _consumer.Close();
        }
    }
}