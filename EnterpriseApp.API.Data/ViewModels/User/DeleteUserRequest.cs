using System;
using System.ComponentModel.DataAnnotations;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class DeleteUserRequest
    {
        [Required]
        public string Id { get; set; }
    }
}
