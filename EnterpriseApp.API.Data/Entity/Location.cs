using MongoDB.Bson.Serialization.Attributes;

namespace EnterpriseApp.API.Data.Entity
{
    public class Location
    {
        [BsonElement("divisionName")]
        public string DivisionName { get; set; }

        [BsonElement("divisionId")]
        public string DivisionId { get; set; }

        [BsonElement("districtName")]
        public string DistrictName { get; set; }

        [BsonElement("districtId")]
        public string DistrictId { get; set; }
    }
}