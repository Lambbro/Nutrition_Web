using Nutrition.Models;

namespace Nutrition.Repositories.Interfaces
{
    public interface IScheduleRepository
    {
        Task<Schedule> GetByIDAsync(int id);
        Task CreateNewAsync(Schedule schedule);
        Task DeleteAsync(int id);
        Task UpdateTypeAsync(int id, Schedule.ScheduleType scheduleType);
        Task UpdateAsync(Schedule schedule);

    }
}
