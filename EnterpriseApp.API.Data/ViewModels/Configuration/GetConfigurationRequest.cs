using System;
using System.ComponentModel.DataAnnotations;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class GetConfigurationRequest
    {
        [Required]
        public string UserId { get; set; }
    }
}
