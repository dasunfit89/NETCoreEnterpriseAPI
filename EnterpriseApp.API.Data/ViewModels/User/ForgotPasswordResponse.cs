using System;
namespace EnterpriseApp.API.Data.ViewModels
{
    public class ForgotPasswordResponse : BaseResponse
    {
        public bool IsSuccessful { get; set; }

        public string OtpCode { get; set; }

        public string Email { get; set; }
    }
}
