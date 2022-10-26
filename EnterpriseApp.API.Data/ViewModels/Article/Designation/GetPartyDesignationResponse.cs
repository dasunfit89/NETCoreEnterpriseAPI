using System;
using System.Collections.Generic;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class GetPartyDesignationResponse : BaseResponse
    {
        public List<PartyDesignationModel> Items { get; set; }
    }
}
