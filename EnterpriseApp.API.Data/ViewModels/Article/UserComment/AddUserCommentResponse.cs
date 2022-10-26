using System;
using System.Collections.Generic;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class AddUserCommentResponse : BaseResponse
    {
        public UserCommentModel Item { get; set; }
    }
}
