using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EnterpriseApp.API.Models.ViewModels
{
    public class UserFavouriteRestaurantRequest
    {
        [Range(1, int.MaxValue)]
        public int UserId { get; set; }

        [Range(1, int.MaxValue)]
        public int RestaurantId { get; set; }
         
        [Required]
        public bool IsFavorite { get; set; }

    }
}
