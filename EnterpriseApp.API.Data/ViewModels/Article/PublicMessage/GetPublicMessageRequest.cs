using System;
using System.ComponentModel.DataAnnotations;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class GetPublicMessageRequest
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public int ChatType { get; set; }

        public int MessageType { get; set; }

        public string DistrictId { get; set; }

        public string DivisionId { get; set; }

        public GetPublicMessageRequest()
        {
        }
    }
}
