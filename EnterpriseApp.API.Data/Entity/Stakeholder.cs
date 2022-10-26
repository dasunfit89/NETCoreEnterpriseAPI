using System;
using MongoDB.Bson.Serialization.Attributes;

namespace EnterpriseApp.API.Data.Entity
{
    public class Stakeholder : BaseEntity
    {
        [BsonElement("name")]
        public string Name { get; set; }
    }
}
