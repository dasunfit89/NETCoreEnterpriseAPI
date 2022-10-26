using System;
using Google.Cloud.Firestore;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class BaseEntityModel
    {
        public string Id { get; set; }

        public int Status { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime DeletedOn { get; set; }

        public DateTime UpdatedOn { get; set; }
    }
}
