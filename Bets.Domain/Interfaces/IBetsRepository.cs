using Bets.Domain.Entities;
using Bets.Domain.Enums;

namespace Bets.Domain.Interfaces
{
    public interface IBetsRepository
    {
        Task<Bet> CreateBetAsync(long selectionId, decimal stake, int userId, BetStatusEnum status, DateTime createDate, CancellationToken ct);
        Task<Bet> UpdateBetConfirmationAsync(long betId, BetStatusEnum status, DateTime updateDate, CancellationToken ct);
        Task<List<Bet>> GetUserBetsAsync(int userId, CancellationToken ct);
    }
}