using Bets.API.Models.Bets;
using Bets.Domain.Entities;
using Bets.Domain.Enums;
using Bets.Domain.Exceptions;
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

        public BetsProcessor(IUsersRepository usersRepository, IBetsRepository betsRepository, IMessageProducer<string, BetConfirmRequestMessage> producer)
        {
            _usersRepository = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));
            _betsRepository = betsRepository ?? throw new ArgumentNullException(nameof(betsRepository));
            _producer = producer ?? throw new ArgumentNullException(nameof(producer));
        }

        public async Task<Bet> CreateBetAsync(string userToken, CreateBetModel createModel, CancellationToken ct)
        {
            if (createModel == null) throw new ArgumentNullException(nameof(createModel));

            var user = await _usersRepository.GetUserAsync(userToken, ct);
            var status = user.CheckStakeAllowed(createModel.Stake);

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var bet = createModel.GetBet(user.UserId, status);
            var newBet = await _betsRepository.CreateBetAsync(bet, ct);

            if (newBet.Status == BetStatusEnum.Processing)
            {
                await _producer.ProduceAsync(user.UserId.ToString(), newBet.ToConfirmMessage(), ct);
            }

            scope.Complete();
            return newBet;
        }

        public async Task<IEnumerable<BetResponse>> GetUserBetsAsync(string userToken, CancellationToken ct)
        {
            var user = await _usersRepository.GetUserAsync(userToken, ct);
            var userBets = await _betsRepository.GetUserBetsAsync(user.UserId, ct);
            return userBets.Select(a => new BetResponse(a)).ToList();
        }

        public async Task<BetStatusEnum> ProcessBetStatusAsync(UpdateBetStatusModel model, CancellationToken ct)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            await _betsRepository.UpdateBetConfirmationAsync(model, ct);
            return model.NewStatus;
        }
    }
}
