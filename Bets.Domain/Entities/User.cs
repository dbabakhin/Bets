namespace Bets.Domain.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public decimal BetLimit { get; set; }
        public bool CheckStakeAllowed(decimal stake) => BetLimit >= stake;
    }
}
