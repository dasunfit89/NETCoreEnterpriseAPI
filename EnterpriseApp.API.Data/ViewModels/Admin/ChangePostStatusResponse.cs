using System;
namespace EnterpriseApp.API.Data.ViewModels
{
    public class ChangePostStatusResponse : BaseResponse
    {
        public bool IsSuccessful { get; set; }

        public ChangePostStatusResponse()
        {
        }
    }
}
