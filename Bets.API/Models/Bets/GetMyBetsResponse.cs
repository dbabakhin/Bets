using Bets.Domain.Entities;

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
            Status = bet.Status.ToString();
        }


        public long SelectionId { get; set; }
        public decimal Stake { get; set; }
        public string Status { get; set; }
    }
}
