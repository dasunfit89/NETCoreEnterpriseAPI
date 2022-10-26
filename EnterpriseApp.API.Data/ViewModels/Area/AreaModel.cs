using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class AreaModel : BaseEntityModel
    {
        public string Name { get; set; }

        public List<DistrictModel> Districts { get; set; }

        public AreaModel()
        {
            Districts = new List<DistrictModel>();
        }
    }
}
