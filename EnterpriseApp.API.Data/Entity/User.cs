using System;
using System.Collections.Generic;
using EnterpriseApp.API.Data.ViewModels;
using MongoDB.Bson.Serialization.Attributes;

namespace EnterpriseApp.API.Data.Entity
{
    public class User : BaseEntity
    {
        [BsonElement("titleId")]
        public string TitleId { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("password")]
        public string Password { get; set; }

        [BsonElement("profilePic")]
        public string ProfilePic { get; set; }

        [BsonElement("firstName")]
        public string FirstName { get; set; }

        [BsonElement("lastName")]
        public string LastName { get; set; }

        [BsonElement("otp")]
        public string OTP { get; set; }

        [BsonElement("gender")]
        public string Gender { get; set; }

        [BsonElement("dateOfBirth")]
        public DateTime DateOfBirth { get; set; }

        [BsonElement("phone")]
        public string Phone { get; set; }

        [BsonElement("address")]
        public string Address { get; set; }

        [BsonElement("userType")]
        public int UserType { get; set; }

        [BsonElement("stakeholderId")]
        public string StakeholderId { get; set; }

        [BsonElement("permissions")]
        public List<string> Permissions { get; set; }

        [BsonElement("nic")]
        public string Nic { get; set; }

        [BsonElement("occupation")]
        public string Occupation { get; set; }

        [BsonElement("myFiles")]
        public List<FileUpload> MyFiles { get; set; }

        [BsonElement("quickIntro")]
        public string QuickIntro { get; set; }

        [BsonElement("location")]
        public Location Location { get; set; }

        public User()
        {
            MyFiles = new List<FileUpload>();
        }
    }
}
