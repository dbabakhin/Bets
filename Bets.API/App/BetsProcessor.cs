using Bets.API.Models.Bets;
using Bets.Domain.Entities;
using Bets.Domain.Enums;
using Bets.Domain.Exceptions;
using Bets.Domain.Interfaces;
using Shared.Common.Interfaces;
using Shared.Common.Messages;

namespace Bets.API.App
{
    public class BetsProcessor
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

        public async Task<Bet> CreateBetAsync(string userToken, long selectionId, decimal stake, CancellationToken ct)
        {
            var user = await _usersRepository.GetUserAsync(userToken, ct) ?? throw new UnknownEntityException(typeof(User), userToken);

            var status = user.CheckStakeAllowed(stake) ? BetStatusEnum.Processing : BetStatusEnum.Rejected;

            var newBet = await _betsRepository.CreateBetAsync(selectionId, stake, user.UserId, status, DateTime.UtcNow, ct);

            if (newBet.Status == BetStatusEnum.Processing)
            {
                await _producer.ProduceAsync(null, newBet.ToConfirmMessage(), ct);
            }

            return newBet;
        }

        public async Task<List<BetResponse>> GetUserBetsAsync(string userToken, CancellationToken ct)
        {
            var user = await _usersRepository.GetUserAsync(userToken, ct);
            var userBets = await _betsRepository.GetUserBetsAsync(user.UserId, ct);
            return userBets.Select(a => new BetResponse(a)).ToList();
        }

        public async Task<BetStatusEnum> ProcessBetStatusAsync(long betId, bool allowed, CancellationToken ct)
        {
            var status = allowed ? BetStatusEnum.Confirmed : BetStatusEnum.Rejected;
            await _betsRepository.UpdateBetConfirmationAsync(betId, status, DateTime.UtcNow, ct);
            return status;
        }
    }
}
