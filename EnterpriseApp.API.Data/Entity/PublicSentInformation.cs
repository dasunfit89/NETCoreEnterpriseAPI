using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace EnterpriseApp.API.Data.Entity
{
    public class PublicSentInformation : BaseEntity
    {
        [BsonElement("categoryId")]
        public string CategoryId { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("images")]
        public List<FileUpload> Images { get; set; }

        [BsonElement("districtId")]
        public string DistrictId { get; set; }

        [BsonElement("divisionId")]
        public string DivisionId { get; set; }

        [BsonElement("reporter")]
        public User Reporter { get; set; }

        [BsonElement("userId")]
        public string UserId { get; set; }

        [BsonElement("location")]
        public Location Location { get; set; }

        [BsonElement("thumbUrl")]
        public string ThumbUrl { get; set; }

        [BsonElement("userComments")]
        public List<UserComment> UserComments { get; set; }

        public PublicSentInformation()
        {
            UserComments = new List<UserComment>();

            Images = new List<FileUpload>();
        }
    }
}
