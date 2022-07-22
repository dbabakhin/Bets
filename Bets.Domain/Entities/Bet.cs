using Bets.Domain.Enums;
using Shared.Common.Messages;

namespace Bets.Domain.Entities
{
    public class Bet
    {
        public Bet(int userId, long selectionId, decimal stake, BetStatusEnum status)
        {
            if (stake <= 0) throw new ArgumentOutOfRangeException(nameof(stake));
            if (userId <= 0) throw new ArgumentOutOfRangeException(nameof(userId));
            if (selectionId <= 0) throw new ArgumentOutOfRangeException(nameof(stake));

            UserId = userId;
            SelectionId = selectionId;
            Stake = stake;
            Status = status;
        }

        public long BetId { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public DateTime UpdatedDate { get; private set; }

        public int UserId { get; private set; }
        public long SelectionId { get; private set; }
        public decimal Stake { get; private set; }
        public BetStatusEnum Status { get; private set; }

        public void SetId(long betId)
        {
            if (betId <= 0) throw new ArgumentOutOfRangeException(nameof(betId));

            BetId = betId;
        }

        public BetConfirmRequestMessage ToConfirmMessage() => new BetConfirmRequestMessage()
        {
            BetId = BetId,
            SelectionId = SelectionId,
            Stake = Stake,
            UserId = UserId
        };
    }
}
