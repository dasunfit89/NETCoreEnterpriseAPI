using System.ComponentModel.DataAnnotations;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class ForgotPasswordRequest
    {
        [Required]
        [RegularExpression(APIConstants.EMAIL_REGEX, ErrorMessage = APIConstants.EMAIL_REGEX_ERROR)]
        public string Email { get; set; }
    }
}
