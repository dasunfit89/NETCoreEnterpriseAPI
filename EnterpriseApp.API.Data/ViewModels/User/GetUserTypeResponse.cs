using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class GetUserTypeResponse : BaseResponse
    {
        [JsonProperty("items")]
        public List<StakeholderModel> Items { get; set; }

        public GetUserTypeResponse()
        {
            Items = new List<StakeholderModel>();
        }
    }
}
