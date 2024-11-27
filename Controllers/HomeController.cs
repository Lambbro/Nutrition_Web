using Microsoft.AspNetCore.Mvc;
using Nutrition.Models;
using System.Diagnostics;

namespace Nutrition.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var accountId = HttpContext.Session.GetInt32("AccountId");
            var accountType = HttpContext.Session.GetString("AccountType");

            bool isAdmin = false;
            if (accountType != null)
            {
                bool.TryParse(accountType, out isAdmin);
            }

            ViewData["AccountId"] = accountId;
            ViewData["AccountType"] = isAdmin;

            return View();
        }

        public IActionResult About()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
    }
}
