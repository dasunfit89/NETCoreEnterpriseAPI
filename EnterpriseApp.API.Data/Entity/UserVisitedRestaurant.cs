namespace EnterpriseApp.API.Models.Entity
{
    public class UserVisitedRestaurant : BaseEntity
    {
        public int Id { get; set; }

        public int RestaurantId { get; set; }

        public virtual Restaurant Restaurant { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }
    }
}
