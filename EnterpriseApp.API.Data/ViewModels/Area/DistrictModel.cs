using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class DistrictModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public List<DivisionModel> Divisions { get; set; }

        public DistrictModel()
        {
            Divisions = new List<DivisionModel>();
        }
    }
}