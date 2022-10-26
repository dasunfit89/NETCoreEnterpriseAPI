using System;
using System.ComponentModel.DataAnnotations;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class GetUserProfileRequest
    {
        [Required]
        public string UserId { get; set; }
    }
}
