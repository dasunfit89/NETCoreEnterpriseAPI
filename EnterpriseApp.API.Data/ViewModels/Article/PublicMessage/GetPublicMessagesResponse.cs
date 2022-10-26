using System;
using System.Collections.Generic;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class GetPublicMessagesResponse : BaseResponse
    {
        public List<PublicMessageModel> Items { get; set; }
    }
}
