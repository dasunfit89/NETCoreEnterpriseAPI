using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EnterpriseApp.API.Core.Services;
using EnterpriseApp.API.Data;
using EnterpriseApp.API.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace EnterpriseApp.API.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IApplicationDataService _applicationDataService;
        protected readonly IUserService _userService;
        protected readonly AppSettings _appSettings;

        public BaseController(IApplicationDataService articleService, IUserService userService, IOptions<AppSettings> appSetting)
        {
            _applicationDataService = articleService;
            _userService = userService;
            _appSettings = appSetting.Value;
        }

        /// <summary>
        /// Create the authentication token for users
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Authentication token</returns>
        [NonAction]
        public string CreateToken(UserModel user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, user.Email),
                }),
                Issuer = _appSettings.Issuer,
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            string tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }
}
