using System;
namespace EnterpriseApp.API.Data.ViewModels
{
    public class VerifyTokenResponse
    {
        public string Token { get; set; }

        public UserModel User { get; set; }

    }
}
