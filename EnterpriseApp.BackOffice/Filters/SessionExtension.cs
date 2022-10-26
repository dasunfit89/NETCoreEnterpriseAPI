using System;
using Microsoft.AspNetCore.Http;

namespace EnterpriseApp.BackOffice.Filters
{
    public static class HttpContextExtension
    {
        public static bool IsSignedIn(this HttpContext context)
        {
            byte[] byteArray = null;

            context.Session.TryGetValue("UserID", out byteArray);

            var hasClaim = byteArray?.Length > 0;

            return hasClaim;
        }

        public static string UserName(this HttpContext context)
        {
            byte[] byteArray = null;

            context.Session.TryGetValue("UserName", out byteArray);

            var hasClaim = byteArray?.Length > 0;

            string userName = string.Empty;

            if (hasClaim)
            {
                userName = System.Text.Encoding.UTF8.GetString(byteArray);
            }

            return userName;
        }
    }
}
