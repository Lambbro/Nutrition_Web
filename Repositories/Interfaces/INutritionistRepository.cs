using Nutrition.Models;

namespace Nutrition.Repositories.Interfaces
{
    public interface INutritionistRepository
    {
        IEnumerable<Nutritionist> GetAll();
        Nutritionist GetById(int maKeHoach);
        void Add(Nutritionist keHoach);
        void Update(Nutritionist keHoach);
        void Delete(int maKeHoach);
        void Save();
        IEnumerable<Nutritionist> Search(string searchTerm);

    }
}
