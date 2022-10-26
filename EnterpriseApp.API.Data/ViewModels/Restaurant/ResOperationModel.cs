namespace EnterpriseApp.API.Models.ViewModels
{
    public class ResBadgeModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string IconId { get; set; }

        public int RestaurantId { get; set; }

        public bool Selected { get; set; }
    }
}