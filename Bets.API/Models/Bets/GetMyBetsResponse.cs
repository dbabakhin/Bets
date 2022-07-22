using Bets.Domain.Entities;
using Bets.Domain.Enums;

namespace Bets.API.Models.Bets
{

    public class GetMyBetsResponse
    {
        public GetMyBetsResponse(List<BetResponse> bets)
        {
            Bets = bets;
        }

        public List<BetResponse> Bets { get; }
    }

    public class BetResponse
    {
        public BetResponse(Bet bet)
        {
            SelectionId = bet.SelectionId;
            Stake = bet.Stake;
            Status = bet.Status;
        }

        public long SelectionId { get; }
        public decimal Stake { get; }
        public BetStatusEnum Status { get; }
    }
}
