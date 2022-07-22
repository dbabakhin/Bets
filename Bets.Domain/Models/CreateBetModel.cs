using Bets.Domain.Entities;
using Bets.Domain.Enums;

namespace Bets.Domain.Models
{
    public class CreateBetModel
    {
        public CreateBetModel(long selectionId, decimal stake)
        {
            if (selectionId <= 0) throw new ArgumentOutOfRangeException(nameof(selectionId));
            if (stake <= 0) throw new ArgumentOutOfRangeException(nameof(stake));

            SelectionId = selectionId;
            Stake = stake;
        }

        public long SelectionId { get; }
        public decimal Stake { get; }

        public Bet GetBet(int userId, BetStatusEnum status)
        {
            return new Bet(userId, SelectionId, Stake, status);
        }
    }
}
