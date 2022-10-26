using MongoDB.Bson.Serialization.Attributes;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class LocationModel
    {
        public string DivisionName { get; set; }

        public string DivisionId { get; set; }

        public string DistrictName { get; set; }

        public string DistrictId { get; set; }
    }
}