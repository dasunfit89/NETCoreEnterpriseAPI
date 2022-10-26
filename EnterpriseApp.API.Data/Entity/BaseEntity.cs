using System;
using Google.Cloud.Firestore;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EnterpriseApp.API.Data.Entity
{
    public class BaseEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("status")]
        public int Status { get; set; }

        [BsonElement("created_on")]
        public DateTime CreatedOn { get; set; }

        [BsonElement("updated_on")]
        public DateTime UpdatedOn { get; set; }

        [BsonElement("deleted_on")]
        public DateTime DeletedOn { get; set; }
    }
}
