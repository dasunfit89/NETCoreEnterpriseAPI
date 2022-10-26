using System;
using System.Collections.Generic;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class GetPublicSentInformationResponse : BaseResponse
    {
        public List<PublicSentInformationModel> Items { get; set; }
    }
}
