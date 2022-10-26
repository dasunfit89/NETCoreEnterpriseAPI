using System;
using System.ComponentModel.DataAnnotations;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class SendEmailRequest
    {
        [Required]
        public string Email { get; set; }
    }
}
