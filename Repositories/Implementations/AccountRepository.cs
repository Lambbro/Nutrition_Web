using Microsoft.EntityFrameworkCore;
using Nutrition.Data;
using Nutrition.Models;
using Nutrition.Repositories.Interfaces;

namespace Nutrition.Repositories.Implementations
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AppDbContext _context;

        public AccountRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Account> GetById(int id)
        {
            var account = await _context.accounts.FindAsync(id);
            if (account == null)
            {
                throw new ArgumentException($"Account with id {id} not found.");
            }
            return account;
        }
        public async Task<Account> GetByUsername(string username)
        {
            var account = await _context.accounts
        .FirstOrDefaultAsync(a => a.user_name == username);

            if (account == null)
            {
                return null;
            }

            return account;
        }
        public async Task AddAccount(Account account)
        {
            await _context.accounts.AddAsync(account);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAccount(Account account)
        {
            _context.accounts.Update(account);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAccountType(int id, bool accountType)
        {
            var account = await _context.accounts.FindAsync(id);

            if (account == null)
            {
                throw new KeyNotFoundException($"Account with ID {id} not found.");
            }

            account.account_type = accountType;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAccount(int id)
        {
            var account = await GetById(id);
            if (account != null)
            {
                _context.accounts.Remove(account);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<int> GetUserId(int id)
        {
            var account = await GetById(id);
            if (account != null && account.user_id.HasValue)
            {
                return account.user_id.Value;
            }
            else
            {
                return 0;
            }
        }
        public async Task<int> GetNutritionistId(int id)
        {
            var account = await GetById(id);
            if (account != null && account.nutritionist_id.HasValue)
            {
                return account.nutritionist_id.Value;
            }
            else
            {
                return 0;
            }
        }

        public async Task<ICollection<Account>> GetAllAccounts()
        {
            var accounts = await _context.accounts
                .ToListAsync();
            return accounts;
        }
    }
}
