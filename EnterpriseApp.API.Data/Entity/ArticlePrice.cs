using MongoDB.Bson.Serialization.Attributes;

namespace EnterpriseApp.API.Data.Entity
{
    public class ArticlePrice
    {
        [BsonElement("price")]
        public double Price { get; set; }

        [BsonElement("type")]
        public string Type { get; set; }
    }
}