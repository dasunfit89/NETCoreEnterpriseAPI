using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EnterpriseApp.BackOffice.Filters
{
    public class AppAuthorizeFilter : IAuthorizationFilter
    {
        public AppAuthorizeFilter()
        {
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.IsSignedIn())
            {
                context.Result = new RedirectResult("/auth/login");
            }
        }
    }
}
