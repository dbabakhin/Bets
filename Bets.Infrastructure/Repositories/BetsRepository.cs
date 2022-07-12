using Bets.Domain.Entities;
using Bets.Domain.Enums;
using Bets.Domain.Interfaces;

namespace Bets.Infrastructure.Repositories
{
    public class BetsRepository : IBetsRepository
    {
        public async Task<Bet> CreateBetAsync(long selectionId, decimal stake, int userId, BetStatusEnum status, DateTime createDate, CancellationToken ct)
        {
            var b = new Bet()
            {
                CreatedDate = createDate,
                UpdatedDate = createDate,
                SelectionId = selectionId,
                Stake = stake,
                UserId = userId,
                Status = status
            };
            //_ctx.Add(b);
            //await _ctx.SaveChangesAsync(ct);
            return b;
        }

        public async Task<List<Bet>> GetUserBetsAsync(int userId, CancellationToken ct)
        {
            return await Task.FromResult(new List<Bet>());
        }

        public async Task<Bet> UpdateBetConfirmationAsync(long betId, BetStatusEnum status, DateTime updateDate, CancellationToken ct)
        {
            var b = new Bet();
            b.Status = status;
            b.UpdatedDate = updateDate;
            return b;
        }
    }
}
