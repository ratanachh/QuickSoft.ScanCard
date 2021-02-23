using Microsoft.AspNetCore.Mvc;

namespace QuickSoft.ScanCard.Features.Auth
{
    public class AuthController : Controller
    {
        public IActionResult Register()
        {
            return View();
        }
        
        public IActionResult Logout()
        {
            return Redirect("Login");
        }

        public IActionResult Login()
        {
            return View();
        }
    }
}