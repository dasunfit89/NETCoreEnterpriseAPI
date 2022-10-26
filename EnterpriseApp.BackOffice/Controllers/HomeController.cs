using EnterpriseApp.API.Core.Exceptions;
using EnterpriseApp.API.Core.Services;
using EnterpriseApp.API.Core.Services.Interface;
using EnterpriseApp.API.Models;
using EnterpriseApp.API.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
namespace EnterpriseApp.BackOffice.Controllers
{
    //[AppAuthorize()]
    [Route("[controller]")]
    public class HomeController : BackOfficeBaseController
    {
        public HomeController(
            IRestaurantService restaurantService,
            IUserService userService,
            IFileService fileService,
            IOptions<AppSettings> appSettings) : base(restaurantService, userService, fileService, appSettings)
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
