using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class DivisionModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public List<VillageModel> Villages { get; set; }

        public DivisionModel()
        {
            Villages = new List<VillageModel>();
        }
    }
}