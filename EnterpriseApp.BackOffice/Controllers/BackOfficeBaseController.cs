using System.Diagnostics;
using EnterpriseApp.API.Core.Services;
using EnterpriseApp.API.Core.Services.Interface;
using EnterpriseApp.API.Models;
using EnterpriseApp.BackOffice.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace EnterpriseApp.BackOffice.Controllers
{
    public class BackOfficeBaseController : Controller
    {
        protected readonly IRestaurantService _restaurantService;
        protected readonly IUserService _userService;
        protected readonly IFileService _fileService;
        protected readonly AppSettings _appSettings;

        public BackOfficeBaseController(IRestaurantService restaurantService, IUserService userService, IFileService fileService, IOptions<AppSettings> appSetting)
        {
            _restaurantService = restaurantService;
            _userService = userService;
            _fileService = fileService;
            _appSettings = appSetting.Value;
        }

        [AllowAnonymous]
        [Route("Error")]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}