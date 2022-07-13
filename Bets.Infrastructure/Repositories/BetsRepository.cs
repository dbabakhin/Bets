using Bets.Domain.Entities;
using Bets.Domain.Enums;
using Bets.Domain.Interfaces;
using Dapper;
using System.Data.SqlClient;

namespace Bets.Infrastructure.Repositories
{
    public class BetsRepository : IBetsRepository
    {
        private readonly string _conn;

        private const string CREATE_QUERY = "INSERT INTO dbo.Bets (CreatedDate, UpdatedDate, SelectionId, Stake, UserId, Status) " +
                                            "VALUES (@CreatedDate, @UpdatedDate, @SelectionId, @Stake, @UserId, @Status);" +
                                            "SELECT CAST(SCOPE_IDENTITY() as int)";

        private const string SELECT_FOR_USER_QUERY = "SELECT BetId, CreatedDate, UpdatedDate, SelectionId, Stake, UserId, Status " +
                                                     "FROM dbo.Bets WHERE UserId = @UserId";


        private const string UPDATE_QUERY = "UPDATE dbo.Bets " +
                                            "SET Status = @Status, " +
                                            "UpdatedDate = @UpdateDate " +
                                            "WHERE BetId = @BetId";

        public BetsRepository(string conn)
        {
            _conn = conn ?? throw new ArgumentNullException(nameof(conn));
        }

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

            using (var db = new SqlConnection(_conn))
            {
                var res = await db.QueryAsync<int>(CREATE_QUERY, b);
                b.BetId = res.First();
            }

            return b;
        }

        public async Task<List<Bet>> GetUserBetsAsync(int userId, CancellationToken ct)
        {
            using (var db = new SqlConnection(_conn))
            {
                var data = await db.QueryAsync<Bet>(SELECT_FOR_USER_QUERY, userId);
                return data.ToList();
            }
        }

        public async Task UpdateBetConfirmationAsync(long betId, BetStatusEnum status, DateTime updateDate, CancellationToken ct)
        {
            using (var db = new SqlConnection(_conn))
            {
                await db.ExecuteAsync(UPDATE_QUERY, new { Status = (int)status, BetId = betId, UpdateDate = updateDate });
            }
        }
    }
}
