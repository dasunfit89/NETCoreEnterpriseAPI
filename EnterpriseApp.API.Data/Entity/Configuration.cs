using System.Collections.Generic;
using Google.Cloud.Firestore;
using EnterpriseApp.API.Data.ViewModels;
using MongoDB.Bson.Serialization.Attributes;

namespace EnterpriseApp.API.Data.Entity
{
    public class Configuration : BaseEntity
    {
        [BsonElement("enableChat")]
        public bool EnableChat { get; set; }

        [BsonElement("homeCaption")]
        public string HomeCaption { get; set; }

        [BsonElement("homeDescription")]
        public string HomeDescription { get; set; }

        [BsonElement("footerCaption")]
        public string FooterCaption { get; set; }

        [BsonElement("footerDescription")]
        public string FooterDescription { get; set; }

        [BsonElement("homeCaptionEn")]
        public string HomeCaptionEn { get; set; }

        [BsonElement("homeDescriptionEn")]
        public string HomeDescriptionEn { get; set; }

        [BsonElement("footerCaptionEn")]
        public string FooterCaptionEn { get; set; }

        [BsonElement("footerDescriptionEn")]
        public string FooterDescriptionEn { get; set; }

        [BsonElement("images")]
        public List<FileUpload> Images { get; set; }

        public Configuration()
        {
            Images = new List<FileUpload>();
        }
    }
}