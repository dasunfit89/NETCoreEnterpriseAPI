using System;
using System.ComponentModel.DataAnnotations;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class CommonFileUploadModelRequest
    {
        [Required, MaxLength(200)]
        public string FileName { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; }

        public string Description { get; set; }

        public int Type { get; set; }

        [Required]
        public string ToEnitityId { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int UploadType { get; set; }

        public CommonFileUploadModelRequest()
        {
            Type = 1;
            Description = string.Empty;
            Name = string.Empty;
        }
    }
}
