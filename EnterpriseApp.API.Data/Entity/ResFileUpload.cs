namespace EnterpriseApp.API.Models.Entity
{
    public class ResFileUpload : BaseEntity
    {
        public int Id { get; set; }

        public string FileName { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Type { get; set; }

        public int RestaurantId { get; set; }

        public virtual Restaurant Restaurant { get; set; }
       
    }
}