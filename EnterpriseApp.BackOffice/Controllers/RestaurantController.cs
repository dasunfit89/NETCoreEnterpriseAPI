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

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EnterpriseApp.BackOffice.Controllers
{
    public class RestaurantController : BackOfficeBaseController
    {
        public RestaurantController(IRestaurantService restaurantService, IUserService userService, IFileService fileService, IOptions<AppSettings> appSetting) : base(restaurantService, userService, fileService, appSetting)
        {
        }

        #region Public

        public IActionResult ViewRestaurants()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetAllRestaurants()
        {
            //Get form data from client side
            var requestFormData = Request.Form;
            string search = requestFormData["search[value]"];
            string draw = requestFormData["draw"];
            string order = requestFormData["order[0][column]"];
            string orderDir = requestFormData["order[0][dir]"];
            int startRec = Convert.ToInt32(requestFormData["start"]);
            int pageSize = Convert.ToInt32(requestFormData["length"]);

            var pagingParam = new PagingQueryParam
            {
                StartingIndex = startRec,
                PageSize = pageSize,
                Filter = search,
                Sort = $"{order}|{orderDir}" //Column|Order
            };

            var restaurants = _restaurantService.GetAllRestaurants(pagingParam);

            dynamic response = new
            {
                Data = restaurants.List,
                Draw = draw,
                RecordsFiltered = restaurants.TotalRecords,
                RecordsTotal = restaurants.TotalRecords
            };

            return Ok(response);
        }

        public IActionResult CreateRestaurant()
        {
            RestaurantRequest request = new RestaurantRequest()
            {
                Country = GetCountry(),
                Days = GetDays(),
                MapIcons = GetSubcategories(),
                Category = GetCategories(),
                BadgesModel = GetBadges(),
                SubcategoriesList = GetSubcategories(),
            };

            if (TempData.ContainsKey("Result"))
            {
                ViewBag.Result = TempData["Result"].ToString();
            }

            return View(request);
        }

        [HttpPost]
        public IActionResult CreateRestaurant([FromBody]RestaurantRequest request)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = _restaurantService.AddRestaurant(request);

                    if (result.IsSuccessful)
                    {
                        TempData["Result"] = "Restaurant successfully Created";
                        return new JsonResult(new { isSuccess = result.IsSuccessful });
                    }
                }
                catch (ApplicationLogicException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View(request);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "This action cannot be performed");
                    return View(request);
                }
            }

            return View(request);
        }

        public IActionResult RestaurantDetails(int id)
        {
            var restaurant = _restaurantService.GetBackOfficeRestaurantById(id);

            RestaurantModel request = new RestaurantModel()
            {
                Id = restaurant.Id,
                RName = restaurant.RName,
                RStreet = restaurant.RStreet,
                RCity = restaurant.RCity,
                RContact = restaurant.RContact,
                RLongitude = restaurant.RLongitude,
                RLatitude = restaurant.RLatitude,
                GooglePlaceId = restaurant.GooglePlaceId,
                RAddress = restaurant.RAddress,
                RDescription = restaurant.RDescription,
                OpeningHours = restaurant.OpeningHours,
                CountryName = restaurant.CountryName,
                RMetro = restaurant.RMetro,
                IsOpen = restaurant.IsOpen,
                RPostalCode = restaurant.RPostalCode,
                RPrice = restaurant.RPrice,
                RPriceDes = restaurant.RPriceDes,
                RRecommendation = restaurant.RRecommendation,
                CategoryName = restaurant.CategoryName,
                SubcategoriesList = restaurant.SubcategoriesList,
                ResBadges = restaurant.ResBadges,
                Session2 = restaurant.Session2,
                Session1 = restaurant.Session1
            };

            return View(request);
        }

        [HttpGet]
        public IActionResult EditRestaurant(int id)
        {
            var restaurant = _restaurantService.GetBackOfficeRestaurantById(id);

            RestaurantUpdateRequest request = new RestaurantUpdateRequest()
            {
                RestaurantId = restaurant.Id,
                RName = restaurant.RName,
                RStreet = restaurant.RStreet,
                RCity = restaurant.RCity,
                RContact = restaurant.RContact,
                RLongitude = restaurant.RLongitude,
                RLatitude = restaurant.RLatitude,
                GooglePlaceId = restaurant.GooglePlaceId,
                RAddress = restaurant.RAddress,
                RDescription = restaurant.RDescription,
                RPostalCode = restaurant.RPostalCode,
                ImagesList = restaurant.Images,
                RRecommendation = restaurant.RRecommendation,
                RMetro = restaurant.RMetro,
                MapIcon = restaurant.MapIcon,
                RPrice = restaurant.RPrice,
                RPriceDes = restaurant.RPriceDes,
                Countries = GetCountry(),
                Days = GetDays(),
                MapIcons = GetSubcategories(),
                Category = GetCategories(),
                OpeningHours = restaurant.OpeningHours,
                OpeningHoursList = restaurant.OpeningHoursList,
                CountryId = restaurant.CountryId,
                CategoryId = restaurant.CategoryId,
                SubcategoriesList = restaurant.SubcategoriesList,
                ResBadges = restaurant.ResBadges,
                Session1 = restaurant.Session1,
                Session2 = restaurant.Session2
            };

            if (TempData.ContainsKey("Result"))
            {
                ViewBag.Result = TempData["Result"].ToString();
            }

            return View(request);
        }

        [HttpPost]
        public IActionResult EditRestaurant([FromBody]RestaurantUpdateRequest request)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = _restaurantService.EditRestaurant(request);

                    if (result.IsSuccessful)
                    {
                        TempData["Result"] = "Restaurant successfully Updated";
                        return new JsonResult(new { isSuccess = result.IsSuccessful });
                    }
                }
                catch (ApplicationLogicException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View(request);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "This action cannot be performed");
                    return View(request);
                }
            }

            return View(request);
        }

        public IActionResult DeleteRestaurant(int id)
        {
            try
            {
                var result = _restaurantService.DeleteRestaurent(id);

                if (result.IsSuccessful)
                {
                    TempData["Result"] = "Restaurant successfully deleted";
                    return new JsonResult(new { isSuccess = result.IsSuccessful });
                }
                else
                    return View();
            }
            catch (ApplicationLogicException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "This action cannot be performed");
                return View();
            }
        }

        [Route("DeleteResFile/{id:int}")]
        public IActionResult DeleteResFile(int id)
        {
            var result = _fileService.DeleteRestaurentFile(id);

            if (result.IsSuccessful)
                return RedirectToAction("EditRestaurant", "Home", new { Id = result.ParentId });
            else
                return View();
        }

        #endregion

        #region Private 

        private List<SelectListItem> GetCountry()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            var countryList = _restaurantService.CountryList();

            foreach (var item in countryList)
            {
                items.Add(new SelectListItem()
                {
                    Text = item.Name,
                    Value = item.Id.ToString()
                });
            }

            return items;
        }

        private List<SelectListItem> GetCategories()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            var resCategoryList = _restaurantService.ResCategoryList();

            foreach (var item in resCategoryList)
            {
                items.Add(new SelectListItem()
                {
                    Text = item.Name,
                    Value = item.Id.ToString()
                });
            }

            return items;
        }

        private List<SelectListItem> GetDays()
        {

            return new List<SelectListItem> {
                new SelectListItem {Text = "Lundi", Value = "1"},
                new SelectListItem {Text = "Mardi", Value = "2"},
                new SelectListItem {Text = "Mercredi", Value = "3"},
                new SelectListItem {Text = "Jeudi", Value = "4"},
                new SelectListItem {Text = "Vendredi", Value = "5"},
                new SelectListItem {Text = "Samedi", Value = "6"},
                new SelectListItem {Text = "Dimanche", Value = "7"}
            };
        }


        private List<SelectListItem> GetSubcategories()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            var subCategoryList = _restaurantService.SubCategoryList();

            foreach (var item in subCategoryList)
            {
                items.Add(new SelectListItem()
                {
                    Text = item.Name,
                    Value = item.Id.ToString()
                });
            }

            return items;
        }

        private List<BadgeModel> GetBadges()
        {
            return _restaurantService.GetBadges();
        }

        #endregion

    }
}
