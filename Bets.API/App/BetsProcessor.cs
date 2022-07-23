using Bets.API.Models.Bets;
using Bets.Domain.Entities;
using Bets.Domain.Enums;
using Bets.Domain.Interfaces;
using Bets.Domain.Models;
using Shared.Common.Interfaces;
using Shared.Common.Messages;
using System.Transactions;

namespace Bets.API.App
{
    public interface IBetsProcessor
    {
        Task<Bet> CreateBetAsync(string userToken, CreateBetModel model, CancellationToken ct);
        Task<IEnumerable<BetResponse>> GetUserBetsAsync(string userToken, CancellationToken ct);
        Task<BetStatusEnum> ProcessBetStatusAsync(UpdateBetStatusModel model, CancellationToken ct);
    }

    public class BetsProcessor : IBetsProcessor
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IBetsRepository _betsRepository;
        private readonly IMessageProducer<string, BetConfirmRequestMessage> _producer;
        private readonly ILogger<BetsProcessor> _logger;

        public BetsProcessor(IUsersRepository usersRepository,
            IBetsRepository betsRepository,
            IMessageProducer<string, BetConfirmRequestMessage> producer,
            ILogger<BetsProcessor> logger)
        {
            _usersRepository = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));
            _betsRepository = betsRepository ?? throw new ArgumentNullException(nameof(betsRepository));
            _producer = producer ?? throw new ArgumentNullException(nameof(producer));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Bet> CreateBetAsync(string userToken, CreateBetModel createModel, CancellationToken ct)
        {
            if (createModel == null) throw new ArgumentNullException(nameof(createModel));
            _logger.LogInformation("Processing new bet for token: {user}", userToken);
            var user = await _usersRepository.GetUserAsync(userToken, ct) ?? throw new UnauthorizedAccessException();
            _logger.LogInformation("User found: {userId}", user.UserId);
            var status = user.CheckStakeAllowed(createModel.Stake);
            _logger.LogInformation("Bet status checked: {status}", status);
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var bet = createModel.GetBet(user.UserId, status);
            var newBet = await _betsRepository.CreateBetAsync(bet, ct);

            if (newBet.Status == BetStatusEnum.Processing)
            {
                var msg = newBet.ToConfirmMessage();
                await _producer.ProduceAsync(msg.MessageKey, msg, ct);
                _logger.LogInformation("Confirmation request sent with key {key}", msg.MessageKey);
            }

            scope.Complete();
            _logger.LogInformation("Bet created with id {betId}", bet.BetId);
            return newBet;
        }

        public async Task<IEnumerable<BetResponse>> GetUserBetsAsync(string userToken, CancellationToken ct)
        {
            _logger.LogInformation("Bet list requested for token: {userToken}", userToken);
            var user = await _usersRepository.GetUserAsync(userToken, ct) ?? throw new UnauthorizedAccessException();

            var userBets = await _betsRepository.GetUserBetsAsync(user.UserId, ct);
            return userBets.Select(a => new BetResponse(a)).ToList();
        }

        public async Task<BetStatusEnum> ProcessBetStatusAsync(UpdateBetStatusModel model, CancellationToken ct)
        {
            _logger.LogInformation("Processing bet status update for betId: {betId} newstatus: {status}", model.BetId, model.NewStatus);
            if (model == null) throw new ArgumentNullException(nameof(model));
            await _betsRepository.UpdateBetConfirmationAsync(model, ct);
            _logger.LogInformation("New bet status saved");
            return model.NewStatus;
        }
    }
}
