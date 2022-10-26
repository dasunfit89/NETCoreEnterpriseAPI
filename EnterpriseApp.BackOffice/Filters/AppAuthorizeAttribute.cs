using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseApp.BackOffice.Filters
{
    public class AppAuthorizeAttribute : TypeFilterAttribute
    {
        public AppAuthorizeAttribute() : base(typeof(AppAuthorizeFilter))
        {

        }
    }
}
