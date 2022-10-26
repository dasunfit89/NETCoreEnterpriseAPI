using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EnterpriseApp.API.Data.Entity
{
    public class Division
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("villages")]
        public List<Village> Villages { get; set; }

        public Division()
        {
            Villages = new List<Village>();

            Id = ObjectId.GenerateNewId().ToString();
        }
    }
}