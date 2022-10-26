using EnterpriseApp.API.Core.Services;
using EnterpriseApp.API.Core.Services.Interface;
using EnterpriseApp.API.Filters;
using EnterpriseApp.API.Models;
using EnterpriseApp.API.Models.Common;
using EnterpriseApp.API.Models.ViewModels;
using EnterpriseApp.API.Models.ViewModels.Restaurant;
using EnterpriseApp.API.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EnterpriseApp.API.Controllers
{
    [Route("api/[controller]")]
    public class RestaurantController : BaseController
    {
        private readonly IEmailServices _emailServices;

        /// <summary>
        /// Restaurant controller constructor 
        /// </summary>
        /// <param name="restaurantService"></param>
        /// <param name="userService"></param>
        /// <param name="appSettings"></param>
        public RestaurantController(
            IRestaurantService restaurantService,
            IUserService userService,
            IOptions<AppSettings> appSettings,
            IEmailServices emailServices) : base(restaurantService, userService, appSettings)
        {
            _emailServices = emailServices;
        }

        /// <summary>
        /// Get Restaurants list with pagination
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("List")]
        [ModelValidationFilter]
        [ProducesResponseType(typeof(CommonErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(PaginatedResponse<RestaurantModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRestaurants([FromQuery] GetRestaurantListRequest request)
        {
            return Ok(await _restaurantService.GetRestaurants(request));
        }

        /// <summary>
        /// Get Restaurants list with paginate
        /// </summary>
        /// <param User_Location_Lat="User_Location_Lat"></param>
        /// <param User_Location_Lon="User_Location_Lon"></param>
        /// <param UserId="UserId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("RestaurantList")]
        [ProducesResponseType(typeof(CommonErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(PaginatedResponse<RestaurantModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> RestaurantList([FromQuery] GetRestaurantListRequest request)
        {
            return Ok(await _restaurantService.GetRestaurantList(request));
        }

        /// <summary>
        /// Get Restaurant by restaurantId  
        /// </summary>
        /// <param name="restaurantId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(CommonErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(PaginatedResponse<RestaurantModel>), StatusCodes.Status200OK)]
        public IActionResult GetRestaurantById(int restaurantId, string userId = null)
        {
            var restaurant = _restaurantService.GetRestaurantById(restaurantId, userId);
            return Ok(new ApiOkResponse(restaurant));
        }

        /// <summary>
        /// Get Restaurant Comments
        /// </summary>
        /// <param name="resId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{resId:int}/Comments")]
        [Authorize]
        [ProducesResponseType(typeof(CommonErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(IEnumerable<ResCommentModel>), StatusCodes.Status200OK)]
        public IActionResult GetRestaurantComments(int resId)
        {
            var restaurantComments = _restaurantService.GetRestaurantComments(resId);
            return Ok(new ApiOkResponse(restaurantComments));
        }

        /// <summary>
        /// Add Restaurant Comment
        /// </summary>
        /// <param RProblems="RProblems"></param>
        /// <param UserId="UserId"></param>
        /// <param RestaurantId="RestaurantId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddComment")]
        [Authorize]
        [ModelValidationFilter]
        [ProducesResponseType(typeof(CommonErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(AddRestaurantCommentResponse), StatusCodes.Status200OK)]
        public IActionResult AddRestaurantComment([FromBody]ResCommentRequest model)
        {
            var restaurantComments = _restaurantService.AddRestaurantComment(model);
            _emailServices.ProblemSendMail(model);
            return Ok(new ApiOkResponse(restaurantComments));
        }

        /// <summary>
        /// Get Restaurants by badge name
        /// </summary>
        /// <param name="badgenName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Badge/{badgenName}")]
        [Authorize] 
        [ProducesResponseType(typeof(CommonErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(IEnumerable<RestaurantModel>), StatusCodes.Status200OK)]
        public IActionResult GetRestaurantsByBadge(string badgenName)
        {
            var restaurants = _restaurantService.GetRestaurantsByBadge(badgenName);
            return Ok(new ApiOkResponse(restaurants));
        }

        /// <summary>
        /// Add Ratings for a Restaurant by User
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddChoice")]
        [Authorize]
        [ModelValidationFilter]
        [ProducesResponseType(typeof(CommonErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(AddUserRestaurentChoiceResponse), StatusCodes.Status200OK)]
        public IActionResult AddUserRestaurantChoice([FromBody]UserRestaurantChoiceRequest model)
        {
            var result = _restaurantService.AddUserRestaurantChoice(model);
            return Ok(new ApiOkResponse(result));
        }

        /// <summary>
        /// Add a request for a restaurant by user
        /// </summary>
        /// <param RName="RName"></param>
        /// <param RStreet="RStreet"></param>
        /// <param RCity="RCity"></param>
        /// <param CountryId="CountryId"></param>
        /// <param UserId="UserId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("RequestRestaurant")]
        [Authorize]
        [ModelValidationFilter]
        [ProducesResponseType(typeof(CommonErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(AddUserRequestedRestaurantResponse), StatusCodes.Status200OK)]
        public IActionResult AddUserRequestedRestaurant([FromBody]UserRequestedRestaurantRequest model)
        {
            var result = _restaurantService.AddUserRequestedRestaurant(model);
            return Ok(new ApiOkResponse(result));
        }

        /// <summary>
        /// Add user restaurant list
        /// </summary>
        /// <param ListName="ListName"></param>
        /// <param IconId="IconId"></param>
        /// <param LColour="LColour"></param>
        /// <param UserId="UserId"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [ModelValidationFilter]
        [Route("MyList/Add")]
        [ProducesResponseType(typeof(CommonErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(AddUserRestaurantListResponse), StatusCodes.Status200OK)]
        public IActionResult AddUserRestaurantList([FromBody] AddUserRestaurantListRequest request)
        {
            var response = _restaurantService.AddUserRestaurantList(request);
            return Ok(new ApiOkResponse(response));
        }

        /// <summary>
        /// Add user restaurent list
        /// </summary>
        /// <param ListId="ListId"></param>
        /// <param RestaurantId="RestaurantId"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [ModelValidationFilter]
        [Route("MyList/AddRestaurant")]
        [ProducesResponseType(typeof(CommonErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(AddUserRestaurantResponse), StatusCodes.Status200OK)]
        public IActionResult AddUserRestaurentList([FromBody] AddUserRestaurantRequest request)
        {
            var response = _restaurantService.AddUserRestaurant(request);
            return Ok(new ApiOkResponse(response));
        }

        /// <summary>
        /// Add user restaurent list
        /// </summary>
        /// <param Id="Id"></param>
        /// <param ListName="ListName"></param>
        /// <param IconId="IconId"></param>
        /// <param LColour="LColour"></param>
        /// <param UserId="UserId"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        [ModelValidationFilter]
        [Route("MyList/Edit")]
        [ProducesResponseType(typeof(CommonErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(EditUserRestaurantListResponse), StatusCodes.Status200OK)]
        public IActionResult AddUserRestaurentList([FromBody] EditUserRestaurantListRequest request)
        {
            var response = _restaurantService.EditUserRestaurantList(request);
            return Ok(new ApiOkResponse(response));
        }

        /// <summary>
        /// Delete user restaurent list by mylistId
        /// </summary>
        /// <param Id="Id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize]
        [ModelValidationFilter]
        [Route("MyList/Delete")]
        [ProducesResponseType(typeof(CommonErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(EditUserRestaurantListResponse), StatusCodes.Status200OK)]
        public IActionResult DeleteUserRestaurentList([FromBody] DeleteUserRestaurantListRequest request)
        {
            EditUserRestaurantListRequest editRequest = new EditUserRestaurantListRequest()
            {
                Id = request.Id,
            };
            var response = _restaurantService.EditUserRestaurantList(editRequest, true);
            return Ok(new ApiOkResponse(response));
        }

        /// <summary>
        /// Get user myList by userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("MyList/UserMyList/{userId:int}")]
        [ProducesResponseType(typeof(CommonErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(List<UserMyListResponse>), StatusCodes.Status200OK)]
        public IActionResult GetUserMyList(int userId)
        {
            var response = _restaurantService.GetUserMyList(userId);
            return Ok(new ApiOkResponse(response));
        }

        /// <summary>
        /// Get User Restaurants by myListId
        /// </summary>
        /// <param name="myListId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("MyList/MyRestaurants/{myListId:int}")]
        [ProducesResponseType(typeof(CommonErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(IEnumerable<RestaurantModel>), StatusCodes.Status200OK)]
        public IActionResult GetUserRestaurants(int myListId)
        {
            var response = _restaurantService.GetMyRestaurants(myListId);
            return Ok(new ApiOkResponse(response));
        }

        /// <summary>
        /// Search returants by geo coordinates and name
        /// </summary>
        /// <param name="name"></param>
        /// <param latitude="latitude"></param>
        /// <param longitude="longitude"></param>
        /// <param UserId="UserId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Search")]
        [ProducesResponseType(typeof(CommonErrorModel), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetSearch(SearchRequest searchRequest)
        {
            var result = await _restaurantService.SearchRestaurentDetail(searchRequest);

            return Ok(new ApiOkResponse(result));
        }

        /// <summary>
        /// Filter Restaurant and apply pagination
        /// </summary>
        /// <param name="request"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Filter")]
        [ProducesResponseType(typeof(CommonErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(PaginatedResponse<RestaurantModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult> FilterRestaurant([FromBody] FilterRestaurantRequest request, [FromQuery] PagingQueryParam param)
        {
            return Ok(await _restaurantService.FilterRestaurants(request, param));
        }

        /// <summary>
        /// Add user favourite restaurant
        /// </summary>
        /// <param UserId="UserId"></param>
        /// <param RestaurantId="RestaurantId"></param>
        /// <param IsFavorite="IsFavorite"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("AddFavourite")]
        [ProducesResponseType(typeof(CommonErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(RestaurantModel), StatusCodes.Status200OK)]
        public ActionResult AddUserFavouriteRestaurant([FromBody] UserFavouriteRestaurantRequest request)
        {
            var result = _restaurantService.AddUserFavouriteRestaurant(request);

            return Ok(new ApiOkResponse(result));
        }

        /// <summary>
        /// Remove user favourite restaurant
        /// </summary>
        /// <param UserId="UserId"></param>
        /// <param RestaurantId="RestaurantId"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("RemoveFavourite")]
        [ProducesResponseType(typeof(CommonErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(RestaurantModel), StatusCodes.Status200OK)]
        public ActionResult RemoveUserFavouriteRestaurant([FromBody] RemoveUserFavouriteRestaurantRequest request)
        {
            var result = _restaurantService.RemoveUserFavouriteRestaurant(request);

            return Ok(new ApiOkResponse(result));
        }

        /// <summary>
        /// Remove user restaurant
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("MyList/RemoveRestaurant")]
        [ProducesResponseType(typeof(CommonErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(DeleteUserRestaurantResponse), StatusCodes.Status200OK)]
        public ActionResult RemoveUserRestaurant([FromBody] DeleteUserRestaurantRequest request)
        {
            var result = _restaurantService.DeleteUserRestaurant(request);

            return Ok(new ApiOkResponse(result));
        }
    }
}