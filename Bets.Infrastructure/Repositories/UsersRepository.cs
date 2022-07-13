using Bets.Domain.Entities;
using Bets.Domain.Interfaces;
using Dapper;
using System.Data.SqlClient;

namespace Bets.Infrastructure.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly string _conn;

        private const string SELECT_QUERY = "SELECT UserId, BetLimit, UserName, Token " +
                                            "FROM [dbo].[Users]" +
                                            "WHERE Token = @token";
        public UsersRepository(string conn)
        {
            _conn = conn ?? throw new ArgumentNullException(nameof(conn));
        }

        public async Task<User> GetUserAsync(string token, CancellationToken ct)
        {
            using (var db = new SqlConnection(_conn))
            {
                var data = await db.QueryAsync<User>(SELECT_QUERY, new {token = token});
                return data.Single();
            }            
        }
    }
}
