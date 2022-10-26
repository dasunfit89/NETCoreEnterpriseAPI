using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EnterpriseApp.API.Helpers
{
    public static class RequestHelper
    {
        public static async Task<string> FormatRequest(HttpRequest request)
        {
            var body = request.Body;
            ///request.EnableRewind();
            request.Body.Position = 0;

            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            var bodyAsText = Encoding.UTF8.GetString(buffer);
            request.Body = body;

            return $"{request.Scheme} {request.Host}{request.Path} {request.QueryString} {bodyAsText}";
        }
    }
}
