using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class SignUpRequest : LoginBaseRequest
    {
        [Required]
        [RegularExpression(APIConstants.EMAIL_REGEX, ErrorMessage = APIConstants.EMAIL_REGEX_ERROR)]
        public string Email { get; set; }

        [Required, MaxLength(200)]
        public string Password { get; set; }

        [Required, MaxLength(200)]
        public string ConfirmPassword { get; set; }

        [Required, MaxLength(10)]
        [RegularExpression(APIConstants.PHONE_REGEX, ErrorMessage = APIConstants.PHONE_REGEX_ERROR)]
        public string Phone { get; set; }

        [StringRange(AllowableValues = new[] { "N", "M", "F", "O" }, ErrorMessage = APIConstants.GENDER_ERROR)]
        public string Gender { get; set; }

        [Required, MaxLength(100)]
        public string FirstName { get; set; }

        [Required, MaxLength(100)]
        public string LastName { get; set; }

        [Required, MaxLength(200)]
        public string Address { get; set; }

        [Required]
        public string TitleId { get; set; }

        [Required]
        public string StakeholderId { get; set; }

        [Required, MaxLength(100)]
        public string Nic { get; set; }

        [Required]
        public string DistrictId { get; set; }

        [Required]
        public string DivisionId { get; set; }

    }
}
