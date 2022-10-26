using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseApp.API.Models.ViewModels.Restaurant
{
    public class AddUserRequestedRestaurantResponse
    {
        public bool IsSuccessful { get; set; }

        public UserRestaurantRequestModel Request { get; set; }
    }
}
