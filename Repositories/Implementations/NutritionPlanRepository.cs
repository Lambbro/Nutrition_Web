using Nutrition.Data;
using Nutrition.Models;
using Nutrition.Repositories.Interfaces;

namespace Nutrition.Repositories.Implementations
{
    public class NutritionPlanRepository: INutritionPlanRepository
    {
        private readonly AppDbContext _context;

        public NutritionPlanRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<NutritionPlan> GetAll() => _context.nutrition_plans.ToList();



        public NutritionPlan GetById(int id)
        {
            return _context.nutrition_plans.FirstOrDefault(k => k.sMaKeHoach == id); // Tìm bản ghi theo ID
        }


        public IEnumerable<NutritionPlan> Search(string searchTerm)
        {
            return _context.nutrition_plans
                .Where(k => k.sMaKeHoach.ToString().Contains(searchTerm) ||
                            k.sMaChuyenGia.ToString().Contains(searchTerm) ||
                            k.sMaNguoiDung.Contains(searchTerm) ||
                            k.sChiTietKeHoach.Contains(searchTerm) ||
                            k.sGioiYThucPham.Contains(searchTerm))
                .ToList();
        }


        public void Add(NutritionPlan keHoach)
        {
            _context.nutrition_plans.Add(keHoach);
        }

        public void Update(NutritionPlan keHoach)
        {
            _context.nutrition_plans.Update(keHoach);
        }

        public void Delete(int id)
        {
            var keHoach = _context.nutrition_plans.FirstOrDefault(k => k.sMaKeHoach == id); // Tìm bản ghi
            if (keHoach != null)
            {
                _context.nutrition_plans.Remove(keHoach); // Xóa nếu tìm thấy
            }
        }




        public void Save() => _context.SaveChanges();
    }
}
