using Microsoft.AspNetCore.Mvc;

namespace QuickSoft.ScanCard.Features.Customer
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}