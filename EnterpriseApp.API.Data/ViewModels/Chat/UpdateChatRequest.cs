using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class UpdateChatRequest
    {
        [Required]
        public List<string> MessageIds { get; set; }

        [Required]
        public string SenderId { get; set; }

        [Required]
        public int Status { get; set; }
    }
}