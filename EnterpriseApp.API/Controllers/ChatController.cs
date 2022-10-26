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
using EnterpriseApp.API.Core.Authorizations;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace EnterpriseApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : BaseController
    {
        private readonly IFileService _fileService;

        public ChatController(
            IApplicationDataService applicationDataService,
            IUserService userService,
            IFileService fileService,
            IOptions<AppSettings> appSettings) : base(applicationDataService, userService, appSettings)
        {
            _fileService = fileService;
        }

        /// <summary>
        /// Get Articles
        /// </summary>
        /// <returns>Articles list</returns>
        [HttpPost]
        [Route("GetNewChat")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetNewChat([FromBody] GetNewChatRequest request)
        {
            var response = await _applicationDataService.GetNewChat(request);
            return Ok(new ApiOkResponse(response));
        }

        [HttpPost]
        [Route("SendChat")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ServiceFilter(typeof(ModelValidationFilterAttribute))]

        public async Task<IActionResult> SendChat([FromBody] ChatInsertModel model)
        {
            var result = await _applicationDataService.SendChat(model);

            return Ok(new ApiOkResponse(result));
        }

        [HttpPost]
        [Route("UpdateChatDeliveryStatus")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ServiceFilter(typeof(ModelValidationFilterAttribute))]

        public async Task<IActionResult> UpdateChatDeliveryStatus([FromBody] UpdateChatRequest request)
        {
            var result = await _applicationDataService.UpdateChatDeliveryStatus(request);

            return Ok(new ApiOkResponse(result));
        }
    }
}
