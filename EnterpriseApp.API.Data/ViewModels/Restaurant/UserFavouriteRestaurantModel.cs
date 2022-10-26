namespace EnterpriseApp.API.Models.ViewModels
{
    public class UserFavouriteRestaurantModel
    {
        public int Id { get; set; }

        public string UEmail { get; set; }
         
        public int RestaurantId { get; set; }

        public string RestaurantName { get; set; }

        public string UFirstName { get; set; }

        public string ULastName { get; set; }

        public int UserId { get; set; }

        public int Status { get; set; }
    }
}