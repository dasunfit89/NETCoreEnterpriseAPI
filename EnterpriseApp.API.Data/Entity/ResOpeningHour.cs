namespace EnterpriseApp.API.Models.Entity
{
    public class ResOpeningHour : BaseEntity
    {
        public int Id { get; set; }

        public string Session { get; set; }

        public string Day { get; set; }

        public string Opens { get; set; }

        public string Closes { get; set; }

        public int RestaurantId { get; set; }

        public virtual Restaurant Restaurant { get; set; }
    }
}