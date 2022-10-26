namespace EnterpriseApp.API.Models.Entity
{
    public class UserRestaurantRequest : BaseEntity
    {
        public int Id { get; set; }

        public string RName { get; set; }

        public string RStreet { get; set; }

        public string RCity { get; set; }

        public int CountryId { get; set; }

        public virtual Country Country { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }
    }
}
