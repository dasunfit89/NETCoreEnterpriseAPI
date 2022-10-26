using System;
using System.ComponentModel.DataAnnotations;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class GetNewChatRequest
    {
        public string LastMessageId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int ChatType { get; set; }
    }
}