using EnterpriseApp.API.Core.Services;
using EnterpriseApp.API.Core.Services.Interface;
using EnterpriseApp.API.Models;
using EnterpriseApp.API.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EnterpriseApp.BackOffice.Controllers
{
    [Route("auth")]
    public class AuthController : BackOfficeBaseController
    {
        public AuthController(
            IRestaurantService restaurantService,
            IUserService userService,
            IFileService fileService,
            IOptions<AppSettings> appSettings) : base(restaurantService, userService, fileService, appSettings)
        {
        }

        [Route("login")]
        public IActionResult Login(string returnUrl)
        {
            return View(new PortalLoginViewModel
            {
                ReturnUrl = returnUrl
            });
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login(PortalLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.ValidateUserCredentialsAsync(model);

                if (user != null)
                {
                    await SimpleLoginAsync(user);

                    if (IsUrlValid(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }

                    return Redirect("/Home");
                }

                ModelState.AddModelError("InvalidCredentials", "Invalid credentials.");
            }
            else
            {
                return Redirect("/Home/Error");
            }

            return View(model);
        }

        private async Task SimpleLoginAsync(PortalUserModel user)
        {
            HttpContext.Session.SetString("UserName", user.UserName);
            HttpContext.Session.SetString("UserID", user.Id.ToString());

            await Task.FromResult(true);
        }

        private bool IsUrlValid(string returnUrl)
        {
            return !string.IsNullOrWhiteSpace(returnUrl)
                   && Uri.IsWellFormedUriString(returnUrl, UriKind.Relative);
        }

        private async Task LoginAsync(PortalUserModel user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),

            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                //AllowRefresh = <bool>,
                // Refreshing the authentication session should be allowed.

                //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                // The time at which the authentication ticket expires. A 
                // value set here overrides the ExpireTimeSpan option of 
                // CookieAuthenticationOptions set with AddCookie.

                //IsPersistent = true,
                // Whether the authentication session is persisted across 
                // multiple requests. When used with cookies, controls
                // whether the cookie's lifetime is absolute (matching the
                // lifetime of the authentication ticket) or session-based.

                //IssuedUtc = <DateTimeOffset>,
                // The time at which the authentication ticket was issued.

                //RedirectUri = <string>
                // The full path or absolute URI to be used as an http 
                // redirect response value.
            };

            var principal = new ClaimsPrincipal(claimsIdentity);

            ((ClaimsIdentity)User.Identity).AddClaims(claims);
        }

        [Route("logout")]
        public async Task<IActionResult> Logout(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            return await Logout(); ;
        }

        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("UserName");
            HttpContext.Session.Remove("UserID");

            await Task.FromResult(true);

            return RedirectToAction("Login", "Auth");
        }
    }
}

