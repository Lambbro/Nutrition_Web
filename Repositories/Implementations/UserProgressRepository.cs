using Microsoft.EntityFrameworkCore;
using Nutrition.Data;
using Nutrition.Models;
using Nutrition.Repositories.Interfaces;

namespace Nutrition.Repositories.Implementations
{
    public class UserProgressRepository: IUserProgressRepository
    {
        private readonly AppDbContext _context;
        public UserProgressRepository(AppDbContext context) {
            _context = context; 
        }

        public async Task CreateNewAsync(UserProgress userProgress)
        {
            await _context.user_progress.AddAsync(userProgress);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var up = await _context.user_progress.FindAsync(id);
            if (up == null)
            {
                throw new ArgumentException($"Not found");
            }
            _context.user_progress.Remove(up);
            await _context.SaveChangesAsync();
        }

        public async Task<UserProgress> GetByIDAsync(int id)
        {
            var up = await _context.user_progress.FindAsync(id);
            if (up == null)
            {
                throw new ArgumentException($"Not found user progress id {id}");
            }
            return up;
        }

        public async Task UpdateAsync(UserProgress userProgress)
        {
            var up = await _context.user_progress.FindAsync(userProgress.progress_id);
            if (up == null)
            {
                throw new ArgumentException("Not found");
            }

            up.health_status = userProgress.health_status;
            up.weight = userProgress.weight;
            up.create_time = userProgress.create_time;
            up.commment = userProgress.commment;

            _context.user_progress.Update(up);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<UserProgress>> GetAllByUserAsync(int user_id)
        {
            var progresses = await _context.user_progress
                .Where(p => p.user_id == user_id)
                .ToListAsync();
            return (ICollection<UserProgress>)progresses;
        }
    }
}
