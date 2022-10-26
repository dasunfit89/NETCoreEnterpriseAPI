using System.Collections.Generic;
using System.Threading.Tasks;
using EnterpriseApp.API.Core.Authorizations;
using EnterpriseApp.API.Core.Services;
using EnterpriseApp.API.Data;
using EnterpriseApp.API.Data.ViewModels;
using EnterpriseApp.API.Filters;
using EnterpriseApp.API.Helpers;
using EnterpriseApp.API.Models;
using EnterpriseApp.API.Models.ViewModels;
using EnterpriseApp.API.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace EnterpriseApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IFileService _fileService;
        private readonly IEmailService _emailService;
        private readonly bool _skipSMS;

        public UserController(
            IApplicationDataService articleService,
            IUserService userService,
            IFileService fileService,
            IOptions<AppSettings> appSettings,
            IEmailService emailService) : base(articleService, userService, appSettings)
        {
            _fileService = fileService;
            _emailService = emailService;
            _skipSMS = true;
        }

        /// <summary>
        /// This will Authenticate the user 
        /// </summary>
        /// <param name="request">Mandatory</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("Login")]
        [ServiceFilter(typeof(ModelValidationFilterAttribute))]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest request)
        {
            var user = await _userService.Authenticate(request);

            string tokenString = string.Empty;

            if (_skipSMS)
            {
                tokenString = CreateToken(user);
            }

            LoginResponse loginResponse = new LoginResponse()
            {
                User = user,
                SkipSmsValidation = _skipSMS,
                Token = tokenString,
            };

            return Ok(new ApiOkResponse(loginResponse));
        }

        [AllowAnonymous]
        [ServiceFilter(typeof(ModelValidationFilterAttribute))]
        [Route("VerifyToken")]
        [HttpPost]
        public async Task<IActionResult> VerifyToken([FromBody] VerifyTokenRequest request)
        {
            var user = await _userService.VerifyToken(request);

            string tokenString = tokenString = CreateToken(user);

            VerifyTokenResponse loginResponse = new VerifyTokenResponse()
            {
                User = user,
                Token = tokenString,
            };

            return Ok(new ApiOkResponse(loginResponse));
        }

        /// <summary>
        /// Register the user
        /// </summary>
        /// <param name="request"></param>
        /// <returns>User details and authentication token</returns>
        [HttpPost]
        [AllowAnonymous]
        [ServiceFilter(typeof(ModelValidationFilterAttribute))]
        [Route("SignUp")]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequest request)
        {
            var user = await _userService.Signup(request);

            SignUpResponse response = new SignUpResponse()
            {
                User = user,
            };

            return Ok(new ApiOkResponse(response));
        }

        /// <summary>
        /// Edits User
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        [AllowAnonymous]
        [HttpPut]
        [ServiceFilter(typeof(ModelValidationFilterAttribute))]
        [Route("EditProfile")]
        public async Task<IActionResult> EditProfile([FromBody] EditUserDetailsRequest request)
        {
            var user = await _userService.EditUserDetails(request);

            EditUserDetailsResponse reponse = new EditUserDetailsResponse()
            {
                User = user,
            };

            return Ok(new ApiOkResponse(reponse));
        }

        /// <summary>
        /// Get user by userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("Profile")]
        [ServiceFilter(typeof(ModelValidationFilterAttribute))]

        public async Task<IActionResult> GetUserProfile([FromBody] GetUserProfileRequest request)
        {
            var user = await _userService.GetUserModel(request.UserId);

            GetUserProfileResponse reponse = new GetUserProfileResponse()
            {
                User = user,
            };

            return Ok(new ApiOkResponse(reponse));
        }

        /// <summary>
        /// Deleting the user 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ServiceFilter(typeof(ModelValidationFilterAttribute))]

        [Route("DeleteUser")]
        [HttpDelete]
        public async Task<IActionResult> DeleteUser([FromBody] DeleteUserRequest request)
        {
            bool isSuccessful = await _userService.DeleteUser(request);

            DeleteUserResponse response = new DeleteUserResponse()
            {
                IsSuccessful = isSuccessful,
            };

            return Ok(new ApiOkResponse(response));
        }

        /// <summary>
        /// Forgot Password Sequence 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [ServiceFilter(typeof(ModelValidationFilterAttribute))]

        [Route("ForgotPassword")]
        [HttpPost]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var response = await _userService.ForgotPassword(request);
            return Ok(new ApiOkResponse(response));
        }

        /// <summary>
        /// Reset user old password with new password
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [ServiceFilter(typeof(ModelValidationFilterAttribute))]

        [Route("ResetPassword")]
        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var resetPasswordResponse = await _userService.ResetPassword(request);

            return Ok(new ApiOkResponse(resetPasswordResponse));
        }

        [AllowAnonymous]
        [ServiceFilter(typeof(ModelValidationFilterAttribute))]

        [Route("CheckPasswordReset")]
        [HttpPost]
        public async Task<IActionResult> CheckPasswordReset([FromBody] ResetPasswordCheckRequests request)
        {
            var response = await _userService.CheckPasswordReset(request);

            return Ok(new ApiOkResponse(response));
        }

        [ServiceFilter(typeof(ModelValidationFilterAttribute))]
        [Route("UpdateProfilePic")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<IActionResult> UpdateProfilePic([FromBody] UpdateProfilePicRequest request)
        {
            var response = await _userService.UpdateProfilePic(request);

            return Ok(new ApiOkResponse(response));
        }

        /// <summary>
        /// Gets Listed Articles for User
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Stakeholders")]
        public async Task<IActionResult> GetStakeholders()
        {
            var models = await _userService.GetStakeholders();

            return Ok(new ApiOkResponse(models));
        }

        /// <summary>
        /// Gets Listed Articles for User
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("UpdateUserPermissions")]
        [Authorize(Policy = Permissions.SuperUser, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<IActionResult> UpdateUserPermissions([FromBody] UpdatePermissionRequest request)
        {
            var response = await _userService.UpdateUserPermissions(request);

            return Ok(new ApiOkResponse(response));
        }

        /// <summary>
        /// Gets Listed Articles for User
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("EmailForgotPassword")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<IActionResult> EmailForgotPassword([FromBody] SendEmailRequest request)
        {
            var response = await _emailService.EmailOTP(request);

            return Ok(new ApiOkResponse(response));
        }
    }
}