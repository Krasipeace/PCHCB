﻿namespace PCHCB.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using PCHCB.Web.ViewModels.Home;

    [AllowAnonymous]
    public class HomeController : Controller
    {       
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int statusCode)
        {           
            if (statusCode == 403 || statusCode == 404)
            {
                return this.View("AccessDenied");
            }

            return this.View();
        }
    }
}