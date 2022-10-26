using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using EnterpriseApp.API.Core.Exceptions;
using EnterpriseApp.API.Helpers;
using EnterpriseApp.API.Responses;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EnterpriseApp.API.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                //AttemptLogRequest(context, ex);

                if (ex != null)
                {
                    if (ex is ApplicationLogicException)
                        await WriteToResponse(HttpStatusCode.BadRequest, context, ex.Message, (ex as ApplicationLogicException).StatusCode);
                    else if (ex is ApplicationDataException)
                        await WriteToResponse(HttpStatusCode.BadRequest, context, ex.Message, (ex as ApplicationDataException).StatusCode);
                    else if (ex is UnauthorizedException)
                        await WriteToResponse(HttpStatusCode.Unauthorized, context, ex.Message, (ex as UnauthorizedException).StatusCode);
                    else if (ex is UnAuthorizeAccessResourceException)
                        await WriteToResponse(HttpStatusCode.Forbidden, context, ex.Message, (ex as UnAuthorizeAccessResourceException).StatusCode);
                    else if (ex is UnhandledException)
                        await WriteToResponse(HttpStatusCode.InternalServerError, context, ex.Message, (ex as UnhandledException).StatusCode);
                    else
                        await WriteToResponse(HttpStatusCode.InternalServerError, context, ex.Message); //TODO log the error

                }
            }
        }

        private async void AttemptLogRequest(HttpContext context, Exception exception)
        {
            try
            {
                if (exception != null)
                {
                    var exString = exception.ToString().Replace(Environment.NewLine, string.Empty);

                    _logger.LogError($"PmZ1RN6Wqn Exception Trace-{exString}");
                }

                var bodyAsText = await RequestHelper.FormatRequest(context.Request);

                if (!string.IsNullOrWhiteSpace(bodyAsText))
                {
                    bodyAsText = bodyAsText.Replace(Environment.NewLine, string.Empty);

                    _logger.LogError($"PmZ1RN6Wqn Request Body Trace-{bodyAsText}");
                }
            }
            catch (Exception ex)
            {
                var exString = ex.ToString().Replace(Environment.NewLine, string.Empty);

                _logger.LogError($"PmZ1RN6Wqn RequestTrace Error-{exString}");
            }
        }

        private async Task WriteToResponse(HttpStatusCode statusCode, HttpContext context, string msg, StatusCode apiStatus = StatusCode.ERROR_Common)
        {
            context.Response.StatusCode = (int)statusCode;

            if (!context.Response.HasStarted)
            {
                context.Response.ContentType = "application/json";
                var response = new CommonErrorModel(apiStatus, msg);
                var json = JsonConvert.SerializeObject(response);
                await context.Response.WriteAsync(json);
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.

    public static class ExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
