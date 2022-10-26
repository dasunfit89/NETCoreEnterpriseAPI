using System;
using System.Collections.Generic;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class PublicMessageAddUpdateResponse : BaseResponse
    {
        public PublicMessageModel Item { get; set; }
    }
}
