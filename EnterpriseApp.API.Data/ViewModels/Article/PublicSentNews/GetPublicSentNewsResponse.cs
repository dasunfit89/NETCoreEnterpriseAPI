using System;
using System.Collections.Generic;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class GetPublicSentNewsResponse : BaseResponse
    {
        public List<PublicSentNewsModel> Items { get; set; }

        public List<PublicMessageModel> MessageItems { get; set; }
    }
}
