using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace EnterpriseApp.API.Data.Entity
{
    public class Area : BaseEntity
    {
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("districts")]
        public List<District> Districts { get; set; }

        public Area()
        {
            Districts = new List<District>();
        }
    }
}
