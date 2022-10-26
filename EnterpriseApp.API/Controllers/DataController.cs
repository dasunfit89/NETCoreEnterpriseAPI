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
    public class DataController : BaseController
    {
        private readonly IFileService _fileService;

        public DataController(
            IApplicationDataService articleService,
            IUserService userService,
            IFileService fileService,
            IOptions<AppSettings> appSettings) : base(articleService, userService, appSettings)
        {
            _fileService = fileService;
        }

        /// <summary>
        /// Get Articles
        /// </summary>
        /// <returns>Categories list</returns>
        [HttpGet]
        [Route("AreaList")]
        public async Task<IActionResult> GetAreas()
        {
            GetAreasRequest request = new GetAreasRequest();

            var models = await _applicationDataService.GetAreas(request);

            return Ok(new ApiOkResponse(models));
        }

        /// <summary>
        /// Gets Listed Articles for User
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetInformationCategories")]
        [ServiceFilter(typeof(ModelValidationFilterAttribute))]
        public async Task<IActionResult> GetInformationCategories(int type)
        {
            var models = await _applicationDataService.GetInformationCategories(type);

            return Ok(new ApiOkResponse(models));
        }

        /// <summary>
        /// Gets Listed Articles for User
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetDesignationCategories")]
        [ServiceFilter(typeof(ModelValidationFilterAttribute))]
        public async Task<IActionResult> GetDesignationCategories()
        {
            var models = await _applicationDataService.GetDesignationCategories();

            return Ok(new ApiOkResponse(models));
        }

        /// <summary>
        /// Get Articles
        /// </summary>
        /// <returns>Articles list</returns>
        [HttpPost]
        [Route("GetPublicSentNews")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetPublicSentNews([FromBody] GetArticleListRequest request)
        {
            var models = await _applicationDataService.GetPublicSentNews(request);
            return Ok(new ApiOkResponse(models));
        }

        [HttpPost]
        [Route("AddPublicSentNews")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ServiceFilter(typeof(ModelValidationFilterAttribute))]
        public async Task<IActionResult> AddPublicSentNews([FromBody] PublicSentNewsInsertModel model)
        {
            var result = await _applicationDataService.AddPublicSentNews(model);

            return Ok(new ApiOkResponse(result));
        }

        [HttpPost]
        [Route("GetPublicSentInformation")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetPublicSentInformation([FromBody] GetArticleListRequest request)
        {
            var models = await _applicationDataService.GetPublicSentInformation(request);
            return Ok(new ApiOkResponse(models));
        }

        [HttpPost]
        [Route("AddPublicSentInformation")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ServiceFilter(typeof(ModelValidationFilterAttribute))]

        public async Task<IActionResult> AddPublicSentInformation([FromBody] PublicSentInformationInsertModel model)
        {
            var result = await _applicationDataService.AddPublicSentInformation(model);

            return Ok(new ApiOkResponse(result));
        }

        [HttpPost]
        [Route("GetPublicSentPartyInformation")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetPublicSentPartyInformation([FromBody] GetArticleListRequest request)
        {
            var models = await _applicationDataService.GetPublicSentPartyInformation(request);
            return Ok(new ApiOkResponse(models));
        }

        [HttpPost]
        [Route("AddPublicSentPartyInformation")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ServiceFilter(typeof(ModelValidationFilterAttribute))]

        public async Task<IActionResult> AddPublicSentPartyInformation([FromBody] PublicSentPartyInformationInsertModel model)
        {
            var result = await _applicationDataService.AddPublicSentPartyInformation(model);

            return Ok(new ApiOkResponse(result));
        }

        [HttpPost]
        [Route("UpdateDeviceToken")]
        [ServiceFilter(typeof(ModelValidationFilterAttribute))]
        public async Task<IActionResult> UpdateDeviceToken([FromBody] UpdateDeviceTokenRequest request)
        {
            var result = await _applicationDataService.UpdateDeviceToken(request);

            return Ok(new ApiOkResponse(result));
        }

        /// <summary>
        /// Get Articles
        /// </summary>
        /// <returns>Articles list</returns>
        [HttpPost]
        [Route("GetPartyDesignations")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetPartyDesignations([FromBody] GetArticleListRequest request)
        {
            var models = await _applicationDataService.GetPartyDesignations(request);
            return Ok(new ApiOkResponse(models));
        }

        [HttpPost]
        [Route("AddUserComment")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ServiceFilter(typeof(ModelValidationFilterAttribute))]
        public async Task<IActionResult> AddUserComment([FromBody] UserCommentInsertModel model)
        {
            var result = await _applicationDataService.AddUserComment(model);

            return Ok(new ApiOkResponse(result));
        }

        [HttpPost]
        [Route("UpdatePublicSentNews")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdatePublicSentNews([FromBody] PublicSentNewsUpdateModel request)
        {
            var result = await _applicationDataService.UpdatePublicSentNews(request);

            return Ok(new ApiOkResponse(result));
        }

        [HttpPost]
        [Route("UpdatePublicSentPartyInformation")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdatePublicSentPartyInformation([FromBody] PublicSentPartyInformationUpdateModel request)
        {
            var result = await _applicationDataService.UpdatePublicSentPartyInformation(request);

            return Ok(new ApiOkResponse(result));
        }

        [HttpPost]
        [Route("UpdatePublicSentInformation")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdatePublicSentInformation([FromBody] PublicSentInformationUpdateModel request)
        {
            var result = await _applicationDataService.UpdatePublicSentInformation(request);

            return Ok(new ApiOkResponse(result));
        }

        [HttpPost]
        [Route("UpdatePublicMessage")]
        [Authorize(Policy = Permissions.ManagePosts, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdatePublicMessage([FromBody] PublicMessageUpdateModel request)
        {
            var result = await _applicationDataService.UpdatePublicMessage(request);

            return Ok(new ApiOkResponse(result));
        }

        [HttpPost]
        [Route("AddPublicMessage")]
        [Authorize(Policy = Permissions.ManagePublicMessages, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ServiceFilter(typeof(ModelValidationFilterAttribute))]
        public async Task<IActionResult> SendGeneralChat([FromBody] PublicMessageInsertModel model)
        {
            var result = await _applicationDataService.AddPublicMessage(model);

            return Ok(new ApiOkResponse(result));
        }

        [HttpPost]
        [Route("GetPublicMessages")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ServiceFilter(typeof(ModelValidationFilterAttribute))]
        public async Task<IActionResult> GetGeneralChat([FromBody] GetPublicMessageRequest request)
        {
            var result = await _applicationDataService.GetPublicMessages(request);

            return Ok(new ApiOkResponse(result));
        }

        /// <summary>
        /// Gets Listed Articles for User
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("GetConfiguration")]
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetConfiguration([FromBody] GetConfigurationRequest request)
        {
            var response = await _applicationDataService.GetConfiguration(request);

            return Ok(new ApiOkResponse(response));
        }

        [HttpPost]
        [Route("ChangePostStatus")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ChangePostStatus([FromBody] ChangePostStatusRequest request)
        {
            var result = await _applicationDataService.ChangePostStatus(request);

            return Ok(new ApiOkResponse(result));
        }
    }
}
