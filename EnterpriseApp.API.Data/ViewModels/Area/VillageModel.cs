using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class VillageModel
    {
        public string Id { get; set; }

        public string GN_Code { get; set; }

        public string Name_In_Sinhala { get; set; }

        public string Name_In_Tamil { get; set; }

        public string Name_In_English { get; set; }

        public VillageModel()
        {

        }
    }
}