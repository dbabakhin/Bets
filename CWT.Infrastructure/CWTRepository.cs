using CWT.Domain.Interfaces;

namespace CWT.Infrastructure
{
    public class CWTRepository : ICWTRepository
    {
        public async Task<decimal> GetUserBalanceAsync(int userId)
        {
            return await Task.FromResult(Random.Shared.Next(0, 1000));
        }
    }
}
