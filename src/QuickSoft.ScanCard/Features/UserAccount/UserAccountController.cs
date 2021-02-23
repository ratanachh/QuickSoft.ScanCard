using Microsoft.AspNetCore.Mvc;

namespace QuickSoft.ScanCard.Features.UserAccount
{
    public class UserAccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}