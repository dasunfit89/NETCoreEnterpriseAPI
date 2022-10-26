using System;
using System.ComponentModel.DataAnnotations;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class VerifyTokenRequest
    {
        [Required]
        [RegularExpression(APIConstants.EMAIL_REGEX, ErrorMessage = APIConstants.EMAIL_REGEX_ERROR)]
        public string Email { get; set; }

        [Required, MaxLength(6)]
        public string OTP { get; set; }

    }
}
