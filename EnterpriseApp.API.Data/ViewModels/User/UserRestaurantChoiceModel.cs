using System;
namespace EnterpriseApp.API.Models.ViewModels
{
    public class UserRestaurantChoiceModel
    {
        public int Id { get; set; }

        public string MyChoice { get; set; }

        public int RestaurantId { get; set; }

        public int UserId { get; set; }
    }
}
