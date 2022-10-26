using System;
namespace EnterpriseApp.API.Models.ViewModels
{
    public class UserVisitedRestaurantModel
    {
        public int Id { get; set; }
        
        public int RestaurantId { get; set; }

        public int UserId { get; set; }

        public string RestaurantName { get; set; }

        public string UFirstName { get; set; }

        public string ULastName { get; set; }
    }
}
