using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseApp.API.Models.ViewModels.Restaurant
{
    public class RestaurantModelSearch
    {
        public List<RestaurantModel> LocalRestaurants { get; set; }

        public List<RestaurantModel> GoogleRestaurants { get; set; }

        public RestaurantModelSearch()
        {
            LocalRestaurants = new List<RestaurantModel>();
            GoogleRestaurants = new List<RestaurantModel>();
        }    
        
    }
}
