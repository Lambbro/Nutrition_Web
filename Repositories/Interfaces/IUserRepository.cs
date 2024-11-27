using Nutrition.Models;

namespace Nutrition.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByIDAsync(int id);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
        Task<ICollection<NutritionPlan>> GetPlanByUserIDAsync (int user_id);
        Task<ICollection<UserProgress>> GetProgessByUserIDAsync(int user_id);
        Task<ICollection<Schedule>> GetSchedulesByUserIdAsync(int user_id);
        Task<ICollection<User>> GetAllUserAsync();

    }
}
