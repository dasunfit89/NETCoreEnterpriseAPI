namespace EnterpriseApp.API.Models.Entity
{
    public class UserVisitedCountries : BaseEntity
    {
        public int Id { get; set; }

        public int CountryId { get; set; }

        public virtual Country Country { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }
    }
}
