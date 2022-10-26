namespace EnterpriseApp.API.Models.ViewModels.Restaurant
{
    public class SearchRequest
    {
        public string name { get; set; }

        public double latitude { get; set; }

        public double longitude { get; set; }

        public string UserId { get; set; }
    }
}
