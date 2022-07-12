namespace CWT.Domain.Interfaces
{
    public interface ICWTRepository
    {
        Task<decimal> GetUserBalanceAsync(int userId);
    }
}