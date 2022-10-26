using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class EditUserDetailsResponse : BaseResponse
    {
        public UserModel User { get; set; }
    }
}
