using System;
namespace EnterpriseApp.API.Models.ViewModels
{
    public class UserRestaurantRequestModel
    {
        public int Id { get; set; }

        public string UEmail { get; set; }

        public string RName { get; set; }

        public string RStreet { get; set; }

        public string RCity { get; set; }

        public int UserId { get; set; }

        public int CountryId { get; set; }
         
        public string CountryName { get; set; }

        public string UFirstName { get; set; }

        public string ULastName { get; set; }
    }
}
