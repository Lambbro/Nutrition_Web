using Nutrition.Models;

namespace Nutrition.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        Task<Account> GetByUsername(string username);
        Task<Account> GetById(int id);
        Task UpdateAccount(Account account);
        Task UpdateAccountType(int id, bool accountType);
        Task AddAccount(Account account);
        Task DeleteAccount(int id);
        Task<int> GetUserId(int id);
        Task<int> GetNutritionistId(int id);
        Task<ICollection<Account>> GetAllAccounts();
    }
}
