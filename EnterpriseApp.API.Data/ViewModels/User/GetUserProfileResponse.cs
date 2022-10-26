using System;
namespace EnterpriseApp.API.Data.ViewModels
{
    public class GetUserProfileResponse : BaseResponse
    {
        public UserModel User { get; set; }
    }
}
