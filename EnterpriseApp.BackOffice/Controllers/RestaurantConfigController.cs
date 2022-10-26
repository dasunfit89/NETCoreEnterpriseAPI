using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EnterpriseApp.API.Core.Services;
using EnterpriseApp.API.Core.Services.Interface;
using EnterpriseApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EnterpriseApp.BackOffice.Controllers
{
    public class RestaurantConfigController : BackOfficeBaseController
    {
        public RestaurantConfigController(IRestaurantService restaurantService, IUserService userService, IFileService fileService, IOptions<AppSettings> appSetting) : base(restaurantService, userService, fileService, appSetting)
        {
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            if (TempData.ContainsKey("Result"))
            {
                ViewBag.Result = TempData["Result"].ToString();
            }
            else if (TempData.ContainsKey("InvalidResult"))
            {
                ViewBag.InvalidResult = TempData["InvalidResult"].ToString();
            }

            return View();
        }

        public IActionResult CheckResturantStatus(int postalCode)
        {
            var result = _restaurantService.CheckResturantStatus(postalCode);

            return new JsonResult(new { status = result, isSuccess = true});
        }

        public IActionResult SetResturantStatus(int postalCode, bool status)
        {
            var result = _restaurantService.ChangeResturantStatus(postalCode, status);

            if(result)
                TempData["Result"] = "Status updated successfully";
            else if(!result)
                TempData["InvalidResult"] = "Invalid postal code";

            return new JsonResult(new { isSuccess = true });
        }
    }
}
