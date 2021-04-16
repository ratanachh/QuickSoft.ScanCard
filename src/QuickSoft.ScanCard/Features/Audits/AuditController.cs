using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace QuickSoft.ScanCard.Features.Audits
{
    [Route("audit")]
    public class AuditController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            var users = new Dictionary<string, string>()
            {
                {"user1", "password1"},
                {"user2", "password2"}
            };
            return Ok(users);
        }
    }
}