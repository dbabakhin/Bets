using Shared.Common.Messages;

namespace CWT.Domain.Interfaces
{
    public interface ICWTService
    {
        Task<BetConfirmResultMessage> CheckEnoughtMoneyAsync(BetConfirmRequestMessage request, CancellationToken ct);
    }
}