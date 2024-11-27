using Nutrition.Models;

namespace Nutrition.Repositories.Interfaces
{
    public interface INutritionPlanRepository
    {
        IEnumerable<NutritionPlan> GetAll();
        NutritionPlan GetById(int maKeHoach);
        void Add(NutritionPlan keHoach);
        void Update(NutritionPlan keHoach);
        void Delete(int maKeHoach);
        void Save();
        IEnumerable<NutritionPlan> Search(string searchTerm);
    }
}
