using Bets.Domain.Entities;

namespace Bets.Domain.Interfaces
{
    public interface IUsersRepository
    {
        Task<User> GetUserAsync(string token, CancellationToken ct);
    }
}
