using Nutrition.Models;

namespace Nutrition.Repositories.Interfaces
{
    public interface IUserProgressRepository
    {
        Task CreateNewAsync(UserProgress userProgress);
        Task UpdateAsync(UserProgress userProgress);
        Task DeleteAsync(int id);
        Task<UserProgress> GetByIDAsync(int id);
        Task<ICollection<UserProgress>> GetAllByUserAsync(int userId);  
        
    }
}
