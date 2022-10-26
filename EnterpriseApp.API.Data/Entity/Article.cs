using System.Collections.Generic;
using Google.Cloud.Firestore;
using EnterpriseApp.API.Data.ViewModels;
using MongoDB.Bson.Serialization.Attributes;

namespace EnterpriseApp.API.Data.Entity
{
    public class Article : BaseEntity
    {
        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("adminArea")]
        public string AdminArea { get; set; }

        [BsonElement("street")]
        public string Street { get; set; }

        [BsonElement("locality")]
        public string Locality { get; set; }

        [BsonElement("houseNo")]
        public string HouseNo { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("address")]
        public string Address { get; set; }

        [BsonElement("coordinates")]
        public Coordinate Coordinates { get; set; }

        [BsonElement("category_id")]
        public string CategoryId { get; set; }

        [BsonElement("sub_category_id")]
        public string SubCategoryId { get; set; }

        [BsonElement("price")]
        public ArticlePrice Price { get; set; }

        [BsonElement("images")]
        public List<FileUpload> Images { get; set; }

        [BsonElement("tags")]
        public List<ArticleTag> Tags { get; set; }

        [BsonElement("user_id")]
        public string UserId { get; set; }
    }
}