using System;
using EnterpriseApp.API.Helpers;
using EnterpriseApp.API.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EnterpriseApp.API.Filters
{
    public sealed class ModelValidationFilterAttribute : ActionFilterAttribute
    {
        public ModelValidationFilterAttribute()
        {

        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                ApiBadRequestResponse apiBadRequestResponse = new ApiBadRequestResponse(context.ModelState);

                context.Result = new BadRequestObjectResult(apiBadRequestResponse);
            }

            base.OnActionExecuting(context);
        }
    }
}
