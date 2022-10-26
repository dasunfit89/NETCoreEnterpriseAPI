using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace EnterpriseApp.API.Models.ViewModels
{
    public class FilterRestaurantRequest
    {
        [JsonProperty("filter1")]
        public List<string> Filter1 { get; set; }

        [JsonProperty("filter2")]
        public List<string> Filter2 { get; set; }

        [JsonProperty("filter3")]
        public List<string> Filter3 { get; set; }

        [JsonProperty("favourite")]
        public bool Favourite { get; set; }

        [JsonProperty("userId")]
        public int UserId { get; set; }

        [JsonProperty("myList")]
        public List<int> MyList { get; set; }

    }
}
