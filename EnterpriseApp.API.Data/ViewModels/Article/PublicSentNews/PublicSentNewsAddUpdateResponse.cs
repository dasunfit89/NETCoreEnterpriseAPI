using System;
using System.Collections.Generic;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class PublicSentNewsAddUpdateResponse : BaseResponse
    {
        public PublicSentNewsModel Item { get; set; }
    }
}
