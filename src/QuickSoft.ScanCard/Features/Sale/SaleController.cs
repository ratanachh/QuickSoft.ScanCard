using Microsoft.AspNetCore.Mvc;

namespace QuickSoft.ScanCard.Features.Sale
{
    public class SaleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}