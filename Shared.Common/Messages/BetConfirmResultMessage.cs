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

        public override string MessageKey => $"{UserId}";

        public long BetId { get; }
        public long SelectionId { get; }
        public decimal Stake { get; }
        public int UserId { get; }
        public bool Allowed { get; }
    }
}
