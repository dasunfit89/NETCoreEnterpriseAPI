using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace EnterpriseApp.API.Data.Entity
{
    public class PartyDesignation : BaseEntity
    {
        [BsonElement("designationId")]
        public string DesignationId { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("images")]
        public List<FileUpload> Images { get; set; }

        [BsonElement("districtId")]
        public string DistrictId { get; set; }

        [BsonElement("divisionId")]
        public string DivisionId { get; set; }

        [BsonElement("thumbUrl")]
        public string ThumbUrl { get; set; }

        [BsonElement("order")]
        public int Order { get; set; }

        public PartyDesignation()
        {

        }
    }
}
