using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace EnterpriseApp.API.Data.Entity
{
    public class Category : BaseEntity
    {
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("sub_categories")]
        public List<SubCategory> SubCategories { get; set; }
    }
}
