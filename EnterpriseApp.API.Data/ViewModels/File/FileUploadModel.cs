using System;
using System.ComponentModel.DataAnnotations;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class FileUploadModel : BaseEntityModel
    {
        [Required, MaxLength(100)]
        public string Filename { get; set; }

        [Required]
        public string Bytes { get; set; }

        public string Url { get; set; }

        public string Description { get; set; }

        [Required, MaxLength(10)]
        public string Type { get; set; }
    }
}
