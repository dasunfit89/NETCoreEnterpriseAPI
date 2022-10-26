using System;
using System.ComponentModel.DataAnnotations;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class DeleteUserRestaurantListRequest
    {
        [Required, Range(1, int.MaxValue)]
        public int Id { get; set; }
    }
}
