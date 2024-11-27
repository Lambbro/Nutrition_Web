using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Nutrition.Data;
using Nutrition.Models;
using Nutrition.Repositories.Interfaces;

namespace Nutrition.Controllers
{
    public class NutritionistController : Controller
    {
        private readonly INutritionistRepository _repository;

        public NutritionistController (INutritionistRepository repository)
        {
            _repository = repository;
        }
        public IActionResult Index()
        {
            var accountType = HttpContext.Session.GetString("AccountType");
            bool isAdmin = false;
            if (accountType != null)
            {
                bool.TryParse(accountType, out isAdmin);
            }
            ViewData["AccountType"] = isAdmin;

            var chuyenGias = _repository.GetAll();
            return View(chuyenGias);
        }
        public IActionResult Create()
        {
            var chuyenGia = new Nutritionist(); // Khởi tạo đối tượng rỗng
            return View(chuyenGia);
        }

        [HttpPost]
        public IActionResult Create(Nutritionist chuyenGia)
        {
            if (ModelState.IsValid)
            {
                _repository.Add(chuyenGia);
                _repository.Save();
                return RedirectToAction("Index");
            }
            return View(chuyenGia);
        }


        public IActionResult Edit(int id)
        {
            var chuyenGia = _repository.GetById(id);
            if (chuyenGia == null) return NotFound();
            return View(chuyenGia);
        }

        [HttpPost]
        public IActionResult Edit(Nutritionist chuyenGia)
        {
            if (ModelState.IsValid)
            {
                var existingChuyenGia = _repository.GetById(chuyenGia.sMaChuyenGia);
                if (existingChuyenGia == null)
                {
                    ModelState.AddModelError("", "Không tìm thấy chuyên gia cần sửa.");
                    return View(chuyenGia);
                }

                // Cập nhật dữ liệu
                existingChuyenGia.sMaChuyenGia = chuyenGia.sMaChuyenGia;
                existingChuyenGia.sHoTenChuyenGia = chuyenGia.sHoTenChuyenGia;
                existingChuyenGia.sSDT = chuyenGia.sSDT;
                existingChuyenGia.sDiaChi = chuyenGia.sDiaChi;
                existingChuyenGia.iTuoi = chuyenGia.iTuoi;
                existingChuyenGia.bGioiTinh = chuyenGia.bGioiTinh;
                existingChuyenGia.sChuyenMon = chuyenGia.sChuyenMon;

                _repository.Save(); // Lưu thay đổi
                TempData["SuccessMessage"] = "Sửa thành công!"; // Gửi thông báo thành công
                return RedirectToAction("Index"); // Quay lại danh sách
            }

            // Nếu dữ liệu không hợp lệ, trả lại View cùng thông báo lỗi
            return View(chuyenGia);
        }

        [HttpPost]
        public IActionResult Delete([FromBody] int id)
        {
            Console.WriteLine($"ID nhận được từ client: {id}"); // Ghi log kiểm tra

            if (string.IsNullOrEmpty(id.ToString()))
            {
                return Json(new { success = false, message = "Mã chuyên gia không hợp lệ" });
            }

            var chuyenGia = _repository.GetById(id);
            if (chuyenGia == null)
            {
                return Json(new { success = false, message = "Không tìm thấy kế hoạch" });
            }

            _repository.Delete(id); // Xóa bản ghi
            _repository.Save(); // Lưu thay đổi
            Console.WriteLine($"Chuyên gia với ID {id} đã được xóa thành công!");
            return Json(new { success = true, message = "Xóa thành công" });
        }
        [HttpGet]
        public IActionResult Search(string searchTerm)
        {
            // Tìm kiếm dữ liệu từ Repository
            var danhSachChuyenGia = string.IsNullOrEmpty(searchTerm)
                ? _repository.GetAll() // Nếu không có từ khóa thì trả về toàn bộ dữ liệu
                : _repository.Search(searchTerm); // Tìm kiếm với từ khóa

            // Trả về JSON
            return Json(danhSachChuyenGia);
        }
    }
}
