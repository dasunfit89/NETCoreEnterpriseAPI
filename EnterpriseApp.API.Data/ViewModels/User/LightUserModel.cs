using System;
using System.Collections.Generic;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class LightUserModel : BaseEntityModel
    {
        public string TitleId { get; set; }

        public string Email { get; set; }

        public string ProfilePic { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Gender { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public int UserType { get; set; }

        public string StakeholderId { get; set; }

        public string Nic { get; set; }

        public List<string> Permissions { get; set; }

        public string Occupation { get; set; }

        public LocationModel Location { get; set; }

        public string QuickIntro { get; set; }

    }
}
