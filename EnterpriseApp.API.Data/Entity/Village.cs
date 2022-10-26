using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EnterpriseApp.API.Data.Entity
{
    public class Village
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("gn_code")]
        public string GN_Code { get; set; }

        [BsonElement("name_in_sinhala")]
        public string Name_In_Sinhala { get; set; }

        [BsonElement("name_in_tamil")]
        public string Name_In_Tamil { get; set; }

        [BsonElement("name_in_english")]
        public string Name_In_English { get; set; }

        public Village()
        {
            Id = ObjectId.GenerateNewId().ToString();
        }
    }
}