using System;
namespace EnterpriseApp.API.Data.ViewModels
{
    public class LoginResponse : BaseResponse
    {
        public UserModel User { get; set; }

        public bool SkipSmsValidation { get; set; }

        public string Token { get; set; }
    }
}
