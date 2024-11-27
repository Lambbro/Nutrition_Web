using Microsoft.AspNetCore.Mvc;
using Nutrition.Models;
using Nutrition.Repositories.Interfaces;

namespace Nutrition.Controllers
{
    public class NutritionPlanController : Controller
    {
        private readonly INutritionPlanRepository _repository;
        public NutritionPlanController (INutritionPlanRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            var keHoachs = _repository.GetAll();
            return View(keHoachs);
        }

        public IActionResult Create()
        {
            var keHoach = new NutritionPlan(); // Khởi tạo đối tượng rỗng
            return View(keHoach);
        }

        [HttpPost]
        public IActionResult Create(NutritionPlan keHoach)
        {
            if (ModelState.IsValid)
            {
                _repository.Add(keHoach);
                _repository.Save();
                return RedirectToAction("Index");
            }
            return View(keHoach);
        }

        public IActionResult Edit(int id)
        {
            var keHoach = _repository.GetById(id);
            if (keHoach == null) return NotFound();
            return View(keHoach);
        }

        [HttpPost]
        public IActionResult Edit(NutritionPlan keHoach)
        {
            if (ModelState.IsValid)
            {
                var existingKeHoach = _repository.GetById(keHoach.sMaKeHoach);
                if (existingKeHoach == null)
                {
                    ModelState.AddModelError("", "Không tìm thấy kế hoạch cần sửa.");
                    return View(keHoach);
                }

                // Cập nhật dữ liệu
                existingKeHoach.sMaChuyenGia = keHoach.sMaChuyenGia;
                existingKeHoach.sMaNguoiDung = keHoach.sMaNguoiDung;
                existingKeHoach.dNgayTao = keHoach.dNgayTao;
                existingKeHoach.sChiTietKeHoach = keHoach.sChiTietKeHoach;
                existingKeHoach.sGioiYThucPham = keHoach.sGioiYThucPham;
                existingKeHoach.iSoCalo = keHoach.iSoCalo;

                _repository.Save(); // Lưu thay đổi
                TempData["SuccessMessage"] = "Sửa thành công!"; // Gửi thông báo thành công
                return RedirectToAction("Index"); // Quay lại danh sách
            }

            return View(keHoach);
        }
        [HttpPost]
        public IActionResult Delete([FromBody] int id)
        {
            Console.WriteLine($"ID nhận được từ client: {id}"); // Ghi log kiểm tra

            if (string.IsNullOrEmpty(id.ToString()))
            {
                return Json(new { success = false, message = "Mã kế hoạch không hợp lệ" });
            }

            var keHoach = _repository.GetById(id);
            if (keHoach == null)
            {
                return Json(new { success = false, message = "Không tìm thấy kế hoạch" });
            }

            _repository.Delete(id); // Xóa bản ghi
            _repository.Save(); // Lưu thay đổi
            Console.WriteLine($"Kế hoạch với ID {id} đã được xóa thành công!");
            return Json(new { success = true, message = "Xóa thành công" });
        }
        [HttpGet]
        public IActionResult Search(string searchTerm)
        {
            // Tìm kiếm dữ liệu từ Repository
            var danhSachKeHoach = string.IsNullOrEmpty(searchTerm)
                ? _repository.GetAll() // Nếu không có từ khóa thì trả về toàn bộ dữ liệu
                : _repository.Search(searchTerm); // Tìm kiếm với từ khóa

            // Trả về JSON
            return Json(danhSachKeHoach);
        }
    }
}
