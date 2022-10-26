using System.Collections.Generic;
using EnterpriseApp.API.Core.Services;
using EnterpriseApp.API.Filters;
using EnterpriseApp.API.Data;
using EnterpriseApp.API.Data.ViewModels;
using EnterpriseApp.API.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using EnterpriseApp.API.Models;
using EnterpriseApp.API.Helpers;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using EnterpriseApp.API.Authorization;
using EnterpriseApp.API.Core.Authorizations;

namespace EnterpriseApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : BaseController
    {
        private readonly IFileService _fileService;

        public AdminController(
            IApplicationDataService articleService,
            IUserService userService,
            IFileService fileService,
            IOptions<AppSettings> appSettings) : base(articleService, userService, appSettings)
        {
            _fileService = fileService;
        }


        [HttpPost]
        [Route("AddPartyDesignation")]
        [Authorize(Policy = Permissions.ManagePartyDesignation, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ServiceFilter(typeof(ModelValidationFilterAttribute))]
        public async Task<IActionResult> AddPublicSentNews([FromBody] PartyDesignationInsertModel model)
        {
            var result = await _applicationDataService.AddPartyDesignation(model);

            return Ok(new ApiOkResponse(result));
        }

        /// <summary>
        /// Get Articles
        /// </summary>
        /// <returns>Articles list</returns>
        [HttpPost]
        [Route("GetPublicSentNews")]
        [Authorize(Policy = Permissions.SuperUser, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetPublicSentNews([FromBody] GetArticleListRequest request)
        {
            request.IsAdmin = true;

            var models = await _applicationDataService.GetPublicSentNews(request);
            return Ok(new ApiOkResponse(models));
        }

        [HttpPost]
        [Route("GetPublicSentInformation")]
        [Authorize(Policy = Permissions.SuperUser, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetPublicSentInformation([FromBody] GetArticleListRequest request)
        {
            request.IsAdmin = true;

            var models = await _applicationDataService.GetPublicSentInformation(request);
            return Ok(new ApiOkResponse(models));
        }

        [HttpPost]
        [Route("GetPublicSentPartyInformation")]
        [Authorize(Policy = Permissions.SuperUser, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetPublicSentPartyInformation([FromBody] GetArticleListRequest request)
        {
            request.IsAdmin = true;

            var models = await _applicationDataService.GetPublicSentPartyInformation(request);
            return Ok(new ApiOkResponse(models));
        }

        [HttpPost]
        [Route("ApprovePost")]
        [Authorize(Policy = Permissions.SuperUser, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ApprovePost([FromBody] ChangePostStatusRequest request)
        {
            var result = await _applicationDataService.ChangePostStatus(request);

            return Ok(new ApiOkResponse(result));
        }

        [HttpPost]
        [Route("GetAdminUsers")]
        [Authorize(Policy = Permissions.SuperUser, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ServiceFilter(typeof(ModelValidationFilterAttribute))]
        public async Task<IActionResult> GetAdminUsers([FromBody] GetAppUsersRequest request)
        {
            var result = await _applicationDataService.GetAdminUsers(request);

            return Ok(new ApiOkResponse(result));
        }


        [HttpPost]
        [Route("GetAppUsers")]
        [Authorize(Policy = Permissions.SuperUser, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ServiceFilter(typeof(ModelValidationFilterAttribute))]
        public async Task<IActionResult> GetAppUsers([FromBody] GetAppUsersRequest request)
        {
            var result = await _applicationDataService.GetAppUsers(request);

            return Ok(new ApiOkResponse(result));
        }

        [HttpPost]
        [Route("UpdateAppUser")]
        [Authorize(Policy = Permissions.SuperUser, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ServiceFilter(typeof(ModelValidationFilterAttribute))]
        public async Task<IActionResult> UpdateAppUser([FromBody] UpdateAppUserRequest request)
        {
            var result = await _applicationDataService.UpdateAppUser(request);

            return Ok(new ApiOkResponse(result));
        }

        [HttpPost]
        [Route("ChangePostStatus")]
        [Authorize(Policy = Permissions.ManagePosts, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ChangePostStatus([FromBody] ChangePostStatusRequest request)
        {
            request.IsFromAdmin = true;

            var result = await _applicationDataService.ChangePostStatus(request);

            return Ok(new ApiOkResponse(result));
        }

        [HttpPost]
        [Route("UpdatePartyDesignation")]
        [Authorize(Policy = Permissions.ManagePosts, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdatePartyDesignation([FromBody] PartyDesignationUpdateModel request)
        {
            var result = await _applicationDataService.UpdatePartyDesignation(request);

            return Ok(new ApiOkResponse(result));
        }

        [HttpPost]
        [Route("UpdateConfig")]
        [Authorize(Policy = Permissions.ManagePosts, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdatePartyDesignation([FromBody] ConfigUpdateRequest request)
        {
            var result = await _applicationDataService.UpdateConfig(request);

            return Ok(new ApiOkResponse(result));
        }
    }
}
