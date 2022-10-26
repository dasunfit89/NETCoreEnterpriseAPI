using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EnterpriseApp.API.Models.ViewModels.Restaurant
{
    public class UserRequestedRestaurantRequest
    {
        [Required]
        public string RName { get; set; }

        [Required]
        public string RStreet { get; set; }

        [Required]
        public string RCity { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int CountryId { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int UserId { get; set; }

    }
}
