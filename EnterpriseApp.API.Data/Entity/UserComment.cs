using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EnterpriseApp.API.Data.Entity
{
    public class UserComment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("userId")]
        public string UserId { get; set; }

        [BsonElement("userName")]
        public string UserName { get; set; }

        [BsonElement("comment")]
        public string Comment { get; set; }

        [BsonElement("status")]
        public int Status { get; set; }

        [BsonElement("created_on")]
        public DateTime CreatedOn { get; set; }
    }
}