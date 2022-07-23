namespace Shared.Common.Messages
{
    public class BetConfirmRequestMessage : MessageBase
    {
        public long BetId { get; set; }
        public long SelectionId { get; set; }
        public decimal Stake { get; set; }
        public int UserId { get; set; }

        public override string MessageKey => $"{UserId}";
    }
}
