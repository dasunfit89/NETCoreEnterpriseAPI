using System;
using System.ComponentModel.DataAnnotations;
using Google.Cloud.Firestore;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class BaseEntityInsertModel
    {
        [Required, MaxLength(20)]
        public string AppVersion { get; set; }

        [Required, MaxLength(20)]
        public string DevicePlatform { get; set; }

        [Required, MaxLength(20)]
        public string Language { get; set; }

        [Required, MaxLength(30)]
        public string IpAddress { get; set; }

        [Required, MaxLength(100)]
        public string Application { get; set; }
    }
}
