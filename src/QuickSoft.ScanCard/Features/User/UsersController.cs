using System;
using Microsoft.AspNetCore.Mvc;

namespace QuickSoft.ScanCard.Features.User
{
    [Route("users")] 
    public class UsersController
    {
        [HttpGet]
        public IActionResult Index()
        {
            throw new NotImplementedException();
        }
    }
}