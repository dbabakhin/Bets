namespace Shared.Common.Messages
{
    public class BetConfirmResultMessage : MessageBase
    {
        public BetConfirmResultMessage(long betId, long selectionId, decimal stake, int userId, bool allowed)
        {
            BetId = betId;
            SelectionId = selectionId;
            Stake = stake;
            UserId = userId;
            Allowed = allowed;
        }

        public long BetId { get; set; }
        public long SelectionId { get; set; }
        public decimal Stake { get; set; }
        public int UserId { get; set; }
        public bool Allowed { get; set; }
    }
}
