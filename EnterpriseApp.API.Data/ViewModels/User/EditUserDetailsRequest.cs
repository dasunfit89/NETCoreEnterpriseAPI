using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class EditUserDetailsRequest : LoginBaseRequest
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        [RegularExpression(APIConstants.EMAIL_REGEX, ErrorMessage = APIConstants.EMAIL_REGEX_ERROR)]
        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        [Required, MaxLength(10)]
        [RegularExpression(APIConstants.PHONE_REGEX, ErrorMessage = APIConstants.PHONE_REGEX_ERROR)]
        public string Phone { get; set; }

        [StringRange(AllowableValues = new[] { "N", "M", "F", "O" }, ErrorMessage = APIConstants.GENDER_ERROR)]
        public string Gender { get; set; }

        public DateTime DateOfBirth { get; set; }

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

        [Required]
        public int UserType { get; set; }

        [Required, MaxLength(100)]
        public string Nic { get; set; }

        public string Occupation { get; set; }

        public List<FileUploadModel> MyFiles { get; set; }

        public string QuickIntro { get; set; }

        public string ProfilePic { get; set; }

        public string DistrictId { get; set; }

        public string DivisionId { get; set; }
    }
}
