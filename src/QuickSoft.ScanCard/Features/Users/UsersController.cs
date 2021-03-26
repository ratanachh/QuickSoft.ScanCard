using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace QuickSoft.ScanCard.Features.Users
{
    [Route("users")] 
    public class UsersController
    {
        [HttpGet]
        public IActionResult Index(List<IFormFile> files)
        {
            throw new NotImplementedException();
        }
    }
}