using Microsoft.AspNetCore.Mvc;

namespace QuickSoft.ScanCard.Features.CardPackage
{
    public class CardPackageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}