using System.ComponentModel.DataAnnotations;
using EnterpriseApp.API.Data;

namespace EnterpriseApp.API.Models.ViewModels
{
    public class FbAuthenticationRequest
    {
        [Required, MaxLength(200)]
        [RegularExpression(APIConstants.EMAIL_REGEX, ErrorMessage = APIConstants.EMAIL_REGEX_ERROR)] 
        public string UEmail { get; set; }
        
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required]
        public string FBId { get; set; }

        public string FBImageURL { get; set; }
    }
}
