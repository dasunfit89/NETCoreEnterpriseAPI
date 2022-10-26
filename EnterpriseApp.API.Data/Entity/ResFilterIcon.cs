namespace EnterpriseApp.API.Models.Entity
{
    public class ResFilterIcon : BaseEntity
    {
        public int Id { get; set; }

        public int RIconID { get; set; }

        public string FilterName { get; set; }

        public int RestaurantId { get; set; }

        public virtual Restaurant Restaurant { get; set; }
    }
}