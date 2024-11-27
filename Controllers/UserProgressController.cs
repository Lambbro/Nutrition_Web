using Microsoft.AspNetCore.Mvc;
using Nutrition.Models;
using Nutrition.Repositories.Interfaces;
using Nutrition.ViewModels;

namespace Nutrition.Controllers
{
    public class UserProgressController : Controller
    {
        private readonly IUserProgressRepository _userProgressRepository;
        private readonly IAccountRepository _accountRepository;
        public UserProgressController(IUserProgressRepository userProgressRepository, IAccountRepository accountRepository)
        {
            _userProgressRepository = userProgressRepository;
            _accountRepository = accountRepository;
        }
        public async Task<IActionResult> List()
        {
            var accountId = HttpContext.Session.GetInt32("AccountId");
            if (!accountId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = await _accountRepository.GetUserId(accountId.Value);
            if (userId == 0)
            {
                return RedirectToAction("CreateUserInfo", "Account", new { account_id = accountId.Value });
            }

            var progresses = await _userProgressRepository.GetAllByUserAsync(userId);

            var viewmodel = progresses
                .OrderByDescending(p => p.create_time)
                .Select(p => new ProgressListViewModel
                {
                    progress_id = p.progress_id,
                    create_time = p.create_time,
                    health_status = p.health_status,
                    weight = p.weight,
                    height = p.height,
                    user_id = p.user_id
                });

            return View(viewmodel);
        }

        public async Task<IActionResult> Details(int id)
        {
            var progress = await _userProgressRepository.GetByIDAsync(id);
            if (progress == null)
            {
                return NotFound();
            }

            var viewModel = new ProgressDetailsViewModel
            {
               progress_id = progress.progress_id,
               user_id = progress.user_id,
               create_time = progress.create_time,
               health_status = progress.health_status,
               height = progress.height,
               weight = progress.weight,
               commment = progress.commment,
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var progress = await _userProgressRepository.GetByIDAsync(id);
            if (progress == null)
            {
                return NotFound();
            }

            var viewModel = new ProgressDetailsViewModel
            {
                progress_id = progress.progress_id,
                user_id = progress.user_id,
                create_time = progress.create_time,
                health_status = progress.health_status,
                height = progress.height,
                weight = progress.weight,
                commment = progress.commment,
            };

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProgressDetailsViewModel viewModel)
        {
            if (id != viewModel.progress_id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var progress = new UserProgress
            {   
                progress_id = viewModel.progress_id,
                user_id = viewModel.user_id,
                health_status = viewModel.health_status,
                height = viewModel.height,
                weight = viewModel.weight,
                commment = viewModel.commment,
                create_time = await _userProgressRepository.GetByIDAsync(id).ContinueWith(t => t.Result.create_time) // Giữ nguyên CreateTime gốc
            };

            await _userProgressRepository.UpdateAsync(progress);
            return RedirectToAction(nameof(List));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProgressDetailsViewModel viewModel)
        {
            var accountId = HttpContext.Session.GetInt32("AccountId");
            if (!accountId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = await _accountRepository.GetUserId(accountId.Value);
            if (userId == 0)
            {
                return RedirectToAction("CreateUserInfo", "Account", new { account_id = accountId.Value });
            }

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var progress = new UserProgress
            {
                progress_id = viewModel.progress_id,
                user_id = userId,
                health_status = viewModel.health_status,
                height = viewModel.height,
                weight = viewModel.weight,
                commment = viewModel.commment,
                create_time = DateTime.UtcNow
            };

            await _userProgressRepository.CreateNewAsync(progress); 
            return RedirectToAction(nameof(List));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var progress = await _userProgressRepository.GetByIDAsync(id);

            return View(progress); // Hiển thị màn hình xác nhận
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var progress = await _userProgressRepository.GetByIDAsync(id);

            // Kiểm tra nếu progress không tồn tại, trả về NotFound
            if (progress == null)
            {
                return NotFound();
            }

            // Gọi phương thức DeleteAsync từ repository để xóa
            await _userProgressRepository.DeleteAsync(id);

            // Chuyển hướng về danh sách UserProgress sau khi xóa thành công
            return RedirectToAction(nameof(List));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAjax(int id)
        {
            var progress = await _userProgressRepository.GetByIDAsync(id);

            if (progress == null)
            {
                return Json(new { success = false, message = "Progress not found" });
            }

            await _userProgressRepository.DeleteAsync(id);

            // Trả về JSON để thông báo thành công
            return Json(new { success = true, message = "Progress deleted successfully" });
        }
    }
}
