using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EnterpriseApp.API.Models.ViewModels
{
    public class UserVisitedRestaurantRequest
    {
        public int CountryId { get; set; }

        [Range(1, int.MaxValue)]
        public int RestaurantId { get; set; }

        [Range(1, int.MaxValue)]
        public int UserId { get; set; }

    }
}
