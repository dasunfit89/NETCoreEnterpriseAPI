using System;
using System.ComponentModel.DataAnnotations;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class ChangePostStatusRequest
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public int Status { get; set; }

        [Required]
        public string PostType { get; set; }

        public bool IsFromAdmin { get; set; }

        public ChangePostStatusRequest()
        {
        }
    }
}
