using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class LoginBaseRequest
    {
        public string DeviceToken { get; set; }

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

    public class LoginRequest : LoginBaseRequest
    {
        [Required]
        [RegularExpression(APIConstants.EMAIL_REGEX, ErrorMessage = APIConstants.EMAIL_REGEX_ERROR)]
        public string Email { get; set; }

        [Required, MaxLength(200)]
        public string Password { get; set; }
    }
}
