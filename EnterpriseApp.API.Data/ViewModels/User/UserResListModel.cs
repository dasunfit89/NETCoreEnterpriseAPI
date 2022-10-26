using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace EnterpriseApp.API.Models.ViewModels
{
    public class UserResListModel
    {
        public int Id { get; set; }

        public string ListName { get; set; }

        public string IconId { get; set; }

        public string LColour { get; set; }

        public int UserId { get; set; }

        [JsonIgnore]
        public List<ResListRestaurantModel> ResListRestaurants { get; set; }

        public List<RestaurantModel> Restaurants { get; set; }

        public int RestaurantCount { get; set; }
    }
}
