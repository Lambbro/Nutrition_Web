using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nutrition.Data;
using Nutrition.Models;
using Nutrition.Repositories.Interfaces;
using Nutrition.ViewModels;

namespace Nutrition.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IUserProgressRepository _userProgressRepository;

        public UserController(IUserRepository userRepository, IAccountRepository accountRepository, IUserProgressRepository userProgressRepository)
        {
            _userRepository = userRepository;
            _accountRepository = accountRepository;
            _userProgressRepository = userProgressRepository;
        }

        public async Task<IActionResult> Profile()
        {
            var accountId = HttpContext.Session.GetInt32("AccountId");
            if (!accountId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }
            var user_id = await _accountRepository.GetUserId(accountId.Value);
            if (user_id == 0)
            {
                return RedirectToAction("CreateUserInfo","Account", new { account_id = accountId.Value });
            }
            var user = await _userRepository.GetByIDAsync(user_id);
            if (user == null)
            {
                return NotFound();
            }

            var progressList = await _userProgressRepository.GetAllByUserAsync(user_id);

            var profileViewModel = new ProfileViewModel
            {
                user_name = user.user_name,
                gender = user.gender,
                age = user.age,
                number = user.number,
                address = user.address,
                height = user.height,
                weight = user.weight,
                health_info = user.health_info,
                eating_habits = user.eating_habits,
                meals_per_day = user.meals_per_day,

                progress_list = progressList.Select(p => new ProgressListViewModel
                {
                    progress_id = p.progress_id,
                    user_id = p.user_id,
                    create_time = p.create_time,
                    height = p.height,
                    weight = p.weight,
                    health_status = p.health_status
                }).ToList()
            };

            return View(profileViewModel);
        }

        public async Task<IActionResult> EditProfile()
        {
            var accountId = HttpContext.Session.GetInt32("AccountId");
            if (!accountId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }
            var user_id = await _accountRepository.GetUserId(accountId.Value);
            if (user_id == 0)
            {
                return RedirectToAction("CreateUserInfo", "Account", new { account_id = accountId.Value });
            }
            var user = await _userRepository.GetByIDAsync(user_id);
            if (user == null)
            {
                return NotFound();
            }
            var editProfileVM = new EditProfileViewModel
            {
                user_name = user.user_name,
                gender = user.gender,
                age = user.age,
                number = user.number,
                address = user.address,
                height = user.height,
                weight = user.weight,
                health_info = user.health_info,
                eating_habits = user.eating_habits,
                meals_per_day = user.meals_per_day
            };
            return View(editProfileVM);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(EditProfileViewModel editProfileVM)
        {
            if (!ModelState.IsValid)
            {
                return View(editProfileVM);
            }
            var accountId = HttpContext.Session.GetInt32("AccountId");
            if (!accountId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }
            var user_id = await _accountRepository.GetUserId(accountId.Value);
            if (user_id == 0)
            {
                return RedirectToAction("CreateUserInfo", "Account", new { account_id = accountId.Value });
            }
            var user = await _userRepository.GetByIDAsync(user_id);
            if (user == null)
            {
                return NotFound();
            }

            user.user_name = editProfileVM.user_name;
            user.number = editProfileVM.number;
            user.weight = editProfileVM.weight;
            user.height = editProfileVM.height;
            user.address = editProfileVM.address;
            user.age = editProfileVM.age;
            user.gender = editProfileVM.gender;
            user.health_info = editProfileVM.health_info;
            user.meals_per_day = editProfileVM.meals_per_day;
            user.eating_habits = editProfileVM.eating_habits;

            await _userRepository.UpdateUserAsync(user);

            return RedirectToAction("Profile");
        }
    }
}
