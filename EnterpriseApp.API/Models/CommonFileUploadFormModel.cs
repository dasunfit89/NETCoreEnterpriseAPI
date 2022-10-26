using EnterpriseApp.API.Data.ViewModels;
using Microsoft.AspNetCore.Http;

namespace EnterpriseApp.API.Models
{
    public class CommonFileUploadFormModel : CommonFileUploadModelRequest
    {
        public IFormFile File { get; set; }
    }
}
