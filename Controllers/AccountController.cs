using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Nutrition.Data;
using Nutrition.Models;
using Nutrition.Repositories.Interfaces;
using Nutrition.ViewModels;

namespace Nutrition.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly INutritionistRepository _nutritionistRepository;

        public AccountController(IUserRepository userRepository, IAccountRepository accountRepository, INutritionistRepository nutritionistRepository)
        {
            _userRepository = userRepository;
            _accountRepository = accountRepository;
            _nutritionistRepository = nutritionistRepository;
        }

        public IActionResult Login()
        {
            return View();  
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel lvm)
        {
            if (ModelState.IsValid)
            {
                var account = await _accountRepository.GetByUsername(lvm.name);
                if (account != null && account.password == lvm.password) // Kiểm tra tài khoản và mật khẩu
                {
                    // Lưu thông tin người dùng vào session hoặc cookie để nhận diện người dùng
                    HttpContext.Session.SetInt32("AccountId", account.account_id);
                    HttpContext.Session.SetString("AccountType", account.account_type.ToString());

                    return RedirectToAction("Index", "Home"); // Redirect đến dashboard chuyên gia dinh dưỡng
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không chính xác.");
                }
            }
            return View(lvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginAjax(LoginViewModel lvm)
        {
            if (ModelState.IsValid)
            {
                var account = await _accountRepository.GetByUsername(lvm.name);
                if (account != null && account.password == lvm.password)
                {
                    // Lưu thông tin vào session
                    HttpContext.Session.SetInt32("AccountId", account.account_id);
                    HttpContext.Session.SetString("AccountType", account.account_type.ToString());

                    return Json(new { success = true, redirectUrl = Url.Action("Index", "Home") });
                }
                else
                {
                    return Json(new { success = false, message = "Tên đăng nhập hoặc mật khẩu không chính xác." });
                }
            }

            return Json(new { success = false, message = "Thông tin không hợp lệ. Vui lòng kiểm tra lại." });
        }


        public IActionResult RedirectToAppointment()
        {
            // Kiểm tra session AccountId
            var accountId = HttpContext.Session.GetInt32("AccountId");

            if (accountId.HasValue && accountId.Value != 0)
            {
                // Nếu đã đăng nhập, chuyển đến trang appointment new
                return RedirectToAction("New", "Appointment");
            }
            else
            {
                // Nếu chưa đăng nhập, chuyển đến trang đăng nhập
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public IActionResult LogOut()
        {
            // Xóa tất cả session liên quan đến người dùng
            HttpContext.Session.Clear();

            // Sau khi xóa session, chuyển hướng về trang chủ hoặc trang login
            return RedirectToAction("Index", "Home"); // Hoặc chuyển hướng đến trang login: return RedirectToAction("Login", "Account");
        }

        public IActionResult Register()
        {
            var account_view_model = new RegisterViewModel();
            return View(account_view_model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel rvm)
        {
            if (ModelState.IsValid)
            {
                var exist_account = await _accountRepository.GetByUsername(rvm.name);
                if (exist_account != null)
                {
                    return View(rvm);
                }
                var account = new Account
                {
                    user_name = rvm.name,
                    password = rvm.password,
                    account_type = false
                    
                };
                await _accountRepository.AddAccount(account);

                return RedirectToAction("CreateUserInfo", new { account.account_id });
            }
            return View(rvm);
        }

        public IActionResult CreateUserInfo(int account_id)
        {
            ViewBag.AccountId = account_id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUserInfo (CreateUserViewModel userVM)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    user_name = userVM.user_name,
                    gender = userVM.gender,
                    age = userVM.age,
                    number = userVM.number,
                    address = userVM.address,
                    height = userVM.height,
                    weight = userVM.weight,
                    health_info = userVM.health_info,
                    eating_habits = userVM.eating_habits,
                    meals_per_day = userVM.meals_per_day
                };

                await _userRepository.AddUserAsync(user);

                var account = await _accountRepository.GetById(userVM.account_id);
                if (account != null)
                {
                    account.user_id = user.user_id;
                    await _accountRepository.UpdateAccount(account);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Account not found.");
                }
                
            }
            return View(userVM);
        }

        
        public async Task<IActionResult> ManageUser()
        {
            var accounts = await _accountRepository.GetAllAccounts();

            var accountType = HttpContext.Session.GetString("AccountType");

            bool isAdmin = false;
            if (accountType != null)
            {
                bool.TryParse(accountType, out isAdmin);
            }
            ViewData["AccountType"] = isAdmin;

            return View(accounts);
        }

        public async Task<IActionResult> EditAccount(int id)
        {
            var account = await _accountRepository.GetById(id);

            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }
        [HttpPost]
        public async Task<IActionResult> EditAccount(Account model)
        {
            if (ModelState.IsValid)
            {
                var account = await _accountRepository.GetById(model.account_id);
                if (account == null)
                {
                    return NotFound();
                };

                // Cập nhật thông tin
                account.user_name = model.user_name;
                account.user_id = model.user_id;
                account.password = model.password;
                account.account_type = model.account_type;
                account.nutritionist_id = model.nutritionist_id;

                await _accountRepository.UpdateAccount(account);

                // Lưu thay đổi vào database

                return RedirectToAction(nameof(ManageUser));
            }
            return View(model);
        }
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var account = await _accountRepository.GetById(id);

            if (account == null)
            {
                return NotFound();
            }

            return View(account); // Hiển thị xác nhận
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int account_id)
        {
            var account = await _accountRepository.GetById(account_id);

            if (account != null)
            {
                if (account.user_id.HasValue)
                {
                    var user = await _userRepository.GetByIDAsync(account.user_id.Value);
                    await _userRepository.DeleteUserAsync(account.user_id.Value);
                }
                if (account.nutritionist_id.HasValue)
                {
                    var user = await _userRepository.GetByIDAsync(account.nutritionist_id.Value);
                    await _userRepository.DeleteUserAsync(account.nutritionist_id.Value);
                }

                await _accountRepository.DeleteAccount(account_id);
            }

            return RedirectToAction("ManageUser");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAjax(int id)
        {
            var account = await _accountRepository.GetById(id);

            try
            {
                if (account != null)
                {
                    if (account.user_id.HasValue)
                    {
                        var user = await _userRepository.GetByIDAsync(account.user_id.Value);
                        if (user != null)
                        {
                            await _userRepository.DeleteUserAsync(account.user_id.Value);
                        }
                    }
                    await _accountRepository.DeleteAccount(id);
                }

                return Json(new { success = true, message = "Deleted Successfully" });
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logger)
                return Json(new { success = false, message = "An error occurred while deleting: " + ex.Message });
            }
        }
    }
}
