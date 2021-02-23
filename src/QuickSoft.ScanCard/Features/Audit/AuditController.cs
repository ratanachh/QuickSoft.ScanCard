using Microsoft.AspNetCore.Mvc;

namespace QuickSoft.ScanCard.Features.Audit
{
    public class AuditController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}