using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace EnterpriseApp.API.Data.Entity
{
    public class UserDevice : BaseEntity
    {
        [BsonElement("pushToken")]
        public string PushToken { get; set; }

        [BsonElement("deviceId")]
        public string DeviceId { get; set; }

        [BsonElement("userId")]
        public string UserId { get; set; }

        [BsonElement("location")]
        public Location Location { get; set; }

    }
}
