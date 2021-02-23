using Microsoft.AspNetCore.Mvc;

namespace QuickSoft.ScanCard.Features.Card
{
    public class CardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}