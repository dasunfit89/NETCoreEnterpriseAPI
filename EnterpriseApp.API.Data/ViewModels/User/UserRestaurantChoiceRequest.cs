using System;
using System.ComponentModel.DataAnnotations;

namespace EnterpriseApp.API.Models.ViewModels
{
    public class UserRestaurantChoiceRequest
    {
        [Required]
        public string MyChoice { get; set; }

        [Range(1, int.MaxValue)]
        public int RestaurantId { get; set; }

        [Range(1, int.MaxValue)]
        public int UserId { get; set; }
    }
}
