using Bets.Domain.Enums;

namespace Bets.Domain.Models
{
    public class UpdateBetStatusModel
    {
        public UpdateBetStatusModel(long betId, bool allowed)
        {
            if (betId <= 0) throw new ArgumentOutOfRangeException(nameof(betId));

            BetId = betId;
            NewStatus = allowed ? BetStatusEnum.Confirmed : BetStatusEnum.Rejected;
        }

        public long BetId { get; }
        public BetStatusEnum NewStatus { get; }
    }
}
