using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EnterpriseApp.API.Data.Entity
{
    public class District
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("divisions")]
        public List<Division> Divisions { get; set; }

        public District()
        {
            Divisions = new List<Division>();

            Id = ObjectId.GenerateNewId().ToString();
        }
    }
}