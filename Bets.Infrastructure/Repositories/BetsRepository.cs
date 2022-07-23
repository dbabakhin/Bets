using Bets.Domain.Entities;
using Bets.Domain.Enums;
using Bets.Domain.Interfaces;
using Bets.Domain.Models;
using Dapper;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;

namespace Bets.Infrastructure.Repositories
{
    public class BetsRepository : IBetsRepository
    {
        private readonly string _conn;

        private const string CREATE_QUERY = "INSERT INTO dbo.Bets (CreatedDate, UpdatedDate, SelectionId, Stake, UserId, Status) " +
                                            "VALUES (GETDATE(), GETDATE(), @SelectionId, @Stake, @UserId, @Status);" +
                                            "SELECT CAST(SCOPE_IDENTITY() as int)";

        private const string SELECT_FOR_USER_QUERY = "SELECT BetId, CreatedDate, UpdatedDate, SelectionId, Stake, UserId, Status " +
                                                     "FROM dbo.Bets WHERE UserId = @UserId";


        private const string UPDATE_QUERY = "UPDATE dbo.Bets " +
                                            "SET Status = @Status, " +
                                            "UpdatedDate = GETDATE() " +
                                            "WHERE BetId = @BetId";

        public BetsRepository(IOptions<BetsConnectionConfig> conn)
        {
            _conn = conn.Value.BetsConnectionString ?? throw new ArgumentNullException(nameof(conn));
        }

        public async Task<Bet> CreateBetAsync(Bet bet, CancellationToken ct)
        {
            if (bet == null) throw new ArgumentNullException(nameof(bet));

            using var db = new SqlConnection(_conn);
            var res = await db.QueryAsync<int>(CREATE_QUERY, bet);
            bet.SetId(res.Single());

            return bet;
        }

        public async Task<List<Bet>> GetUserBetsAsync(int userId, CancellationToken ct)
        {
            using (var db = new SqlConnection(_conn))
            {
                var data = await db.QueryAsync<Bet>(SELECT_FOR_USER_QUERY, userId);
                return data.ToList();
            }
        }

        public async Task UpdateBetConfirmationAsync(UpdateBetStatusModel model, CancellationToken ct)
        {
            using (var db = new SqlConnection(_conn))
            {
                await db.ExecuteAsync(UPDATE_QUERY, new { Status = (int)model.NewStatus, BetId = model.BetId });
            }
        }
    }
}
