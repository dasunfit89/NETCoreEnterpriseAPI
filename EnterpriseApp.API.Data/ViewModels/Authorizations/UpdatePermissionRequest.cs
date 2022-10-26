using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class UpdatePermissionRequest
    {
        [Required]
        public string UserId { get; set; }

        public List<string> Permissions { get; set; }
    }
}
