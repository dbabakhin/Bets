using Bets.Domain.Entities;
using Bets.Domain.Enums;
using Bets.Domain.Models;

namespace Bets.Domain.Interfaces
{
    public interface IBetsRepository
    {
        Task<Bet> CreateBetAsync(Bet bet, CancellationToken ct);
        Task UpdateBetConfirmationAsync(UpdateBetStatusModel model, CancellationToken ct);
        Task<List<Bet>> GetUserBetsAsync(int userId, CancellationToken ct);
    }
}