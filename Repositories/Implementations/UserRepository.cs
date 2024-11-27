using Microsoft.EntityFrameworkCore;
using Nutrition.Data;
using Nutrition.Models;
using Nutrition.Repositories.Interfaces;

namespace Nutrition.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddUserAsync(User user)
        {
            await _context.users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _context.users.FindAsync(id);
            if (user == null)
            {
                throw new ArgumentException($"User with ID {id} not found.");
            }
            _context.users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetByIDAsync(int id)
        {
            var user = await _context.users
                .Include(u => u.schedules)
                .Include(u => u.plans)    
                .Include(u => u.user_progresses)
                .FirstOrDefaultAsync(u => u.user_id == id);
            if (user == null)
            {
                throw new ArgumentException($"User with ID {id} not found.");
            }
            return user;
        }

        public async Task<ICollection<NutritionPlan>> GetPlanByUserIDAsync(int user_id)
        {
            var plans = await _context.nutrition_plans
                
                .ToListAsync();
            return (ICollection<NutritionPlan>)plans;
        }

        public async Task<ICollection<UserProgress>> GetProgessByUserIDAsync(int user_id)
        {
            var progress = await _context.user_progress
                .Where(p => p.user_id == user_id)
                .ToListAsync();
            return (ICollection<UserProgress>)progress;
        }

        public async Task<ICollection<Schedule>> GetSchedulesByUserIdAsync(int user_id)
        {
            var schedules = await _context.schedules
                .Where(p => p.user_id == user_id)
                .ToListAsync();
            return (ICollection<Schedule>)schedules;
        }

        public async Task UpdateUserAsync(User user)
        {
            var exist_user = await _context.users.FindAsync(user.user_id);
            if (exist_user == null)
            {
                throw new ArgumentException($"Not found user");
            }
            exist_user.user_name = user.user_name;
            exist_user.gender = user.gender;
            exist_user.age = user.age;
            exist_user.number = user.number;
            exist_user.address = user.address;
            exist_user.height = user.height;
            exist_user.weight = user.weight;
            exist_user.health_info = user.health_info;
            exist_user.eating_habits = user.eating_habits;
            exist_user.meals_per_day = user.meals_per_day;

            _context.users.Update(exist_user);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<User>> GetAllUserAsync()
        {
            var users = await _context.users
                .Include(u => u.schedules)
                .Include(u => u.plans)
                .Include(u => u.user_progresses)
                .ToListAsync();

            return users;
        }
    }
}
