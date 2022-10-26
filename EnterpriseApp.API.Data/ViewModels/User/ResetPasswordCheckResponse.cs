using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseApp.API.Data.ViewModels
{

    public class ResetPasswordCheckResponse : BaseResponse
    {
        public bool IsSuccessful { get; set; }

        public ResetPasswordCheckResponse()
        {

        }
    }
}
