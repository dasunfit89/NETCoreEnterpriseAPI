using Newtonsoft.Json;
using System;

namespace EnterpriseApp.API.Models
{
    public class UserMyListResponse
    {
        public int ListId { get; set; }

        public string ListName { get; set; }

        public string IconId { get; set; }

        public string LColour { get; set; }

        public int RestaurantCount { get; set; }

        [JsonIgnore]
        public DateTimeOffset? CreatedOn { get; set; }
    }
}
