using System;
using Google.Cloud.Firestore;

namespace EnterpriseApp.API.Models.Entity
{
    [FirestoreData]
    public class City : BaseEntity
    {
        [FirestoreProperty]
        public string Name { get; set; }

        [FirestoreProperty]
        public Coordinate Coordinates { get; set; }
    }
}
