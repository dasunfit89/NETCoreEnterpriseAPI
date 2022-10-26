using System;
using System.ComponentModel.DataAnnotations;

namespace EnterpriseApp.API.Models.ViewModels
{
    public class DeleteUserRestaurantRequest
    {
        [Required, Range(1, int.MaxValue)]
        public int ListId { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int RestaurantId { get; set; }
    }
}
