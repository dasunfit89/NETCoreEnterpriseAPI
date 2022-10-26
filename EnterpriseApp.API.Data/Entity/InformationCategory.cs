using System;
using MongoDB.Bson.Serialization.Attributes;

namespace EnterpriseApp.API.Data.Entity
{
    public class InformationCategory : BaseEntity
    {
        [BsonElement("name-si")]
        public string NameSi { get; set; }

        [BsonElement("order")]
        public int Order { get; set; }

        [BsonElement("type")]
        public int Type { get; set; }
    }
}
