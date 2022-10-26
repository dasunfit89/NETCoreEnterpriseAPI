using System;
namespace EnterpriseApp.API.Models.ViewModels.Restaurant
{
    public class RestaurantInsertResponse
    {
        public bool IsSuccessful { get; set; }

        public RestaurantModel Restaurant { get; set; }
    }
}
