using Bets.Domain.Enums;
using Shared.Common.Messages;

namespace Bets.Domain.Entities
{
    public class Bet
    {
        public long BetId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int UserId { get; set; }
        public long SelectionId { get; set; }
        public decimal Stake { get; set; }
        public BetStatusEnum Status { get; set; }

        public BetConfirmRequestMessage ToConfirmMessage() => new BetConfirmRequestMessage()
        {
            BetId = BetId,
            SelectionId = SelectionId,
            Stake = Stake,
            UserId = UserId
        };
    }
}
