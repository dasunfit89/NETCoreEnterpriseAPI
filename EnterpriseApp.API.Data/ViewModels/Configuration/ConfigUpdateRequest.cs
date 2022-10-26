using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class ConfigUpdateRequest
    {
        [Required]
        public bool EnableChat { get; set; }

        [Required]
        public string HomeCaption { get; set; }

        [Required]
        public string HomeDescription { get; set; }

        [Required]
        public string FooterCaption { get; set; }

        [Required]
        public string FooterDescription { get; set; }

        [Required]
        public string HomeCaptionEn { get; set; }

        [Required]
        public string HomeDescriptionEn { get; set; }

        [Required]
        public string FooterCaptionEn { get; set; }

        [Required]
        public string FooterDescriptionEn { get; set; }

        public List<FileUploadModel> Images { get; set; }
    }
}
