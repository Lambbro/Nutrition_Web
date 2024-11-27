using Nutrition.Data;
using Nutrition.Models;
using Nutrition.Repositories.Interfaces;

namespace Nutrition.Repositories.Implementations
{
    public class NutritionistRepository : INutritionistRepository
    {
        private readonly AppDbContext _context;

        public NutritionistRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Nutritionist> GetAll() => _context.nutritionists.ToList();



        public Nutritionist GetById(int id)
        {
            return _context.nutritionists.FirstOrDefault(k => k.sMaChuyenGia == id); // Tìm bản ghi theo ID
        }


        public IEnumerable<Nutritionist> Search(string searchTerm)
        {
            return _context.nutritionists
                .Where(k => k.sMaChuyenGia.ToString().Contains(searchTerm) ||
                            k.sHoTenChuyenGia.Contains(searchTerm) ||
                            k.sSDT.Contains(searchTerm) ||
                            k.sDiaChi.Contains(searchTerm) ||
                            k.sChuyenMon.Contains(searchTerm))
                .ToList();
        }


        public void Add(Nutritionist chuyenGia)
        {
            _context.nutritionists.Add(chuyenGia);
        }

        public void Update(Nutritionist chuyenGia)
        {
            _context.nutritionists.Update(chuyenGia);
        }

        public void Delete(int id)
        {
            var chuyenGia = _context.nutritionists.FirstOrDefault(k => k.sMaChuyenGia == id); // Tìm bản ghi
            if (chuyenGia != null)
            {
                _context.nutritionists.Remove(chuyenGia); // Xóa nếu tìm thấy
            }
        }

        public void Save() => _context.SaveChanges();
    }
}
