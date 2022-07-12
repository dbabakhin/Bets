using Bets.Domain.Entities;
using Bets.Domain.Interfaces;

namespace Bets.Infrastructure.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        public async Task<User> GetUserAsync(string token, CancellationToken ct)
        {
            var user = new User()
            {
                UserId = token.GetHashCode(),
                BetLimit = Random.Shared.Next(1, 1000)
            };
            return await Task.FromResult(user);
        }
    }
}
