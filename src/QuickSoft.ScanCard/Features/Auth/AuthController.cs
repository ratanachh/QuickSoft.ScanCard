using System;
using Microsoft.AspNetCore.Mvc;

namespace QuickSoft.ScanCard.Features.Auth
{

    public class AuthController : ControllerBase
    {
        public IActionResult Register()
        {
            return Ok();
        }
        

        public IActionResult Logout()
        {
            throw new NotImplementedException();
        }


        public IActionResult Login()
        {
            throw new NotImplementedException();
        }
    }
}