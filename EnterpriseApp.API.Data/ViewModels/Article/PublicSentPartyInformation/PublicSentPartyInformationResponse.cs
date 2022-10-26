using System;
using System.Collections.Generic;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class GetPublicSentPartyNewsResponse : BaseResponse
    {
        public List<PublicSentPartyInformationModel> Items { get; set; }
    }
}
