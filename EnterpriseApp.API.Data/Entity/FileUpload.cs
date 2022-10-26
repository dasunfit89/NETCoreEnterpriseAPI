using MongoDB.Bson.Serialization.Attributes;

namespace EnterpriseApp.API.Data.Entity
{
    public class FileUpload : BaseEntity
    {
        [BsonElement("file_name")]
        public string Filename { get; set; }

        [BsonElement("bytes")]
        public string Bytes { get; set; }

        [BsonElement("url")]
        public string Url { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("type")]
        public string Type { get; set; }
    }
}