using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EnterpriseApp.API.Data.Entity;
using EnterpriseApp.API.Data.ViewModels;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class PublicMessageInsertModel : BaseEntityInsertModel
    {
        [Required]
        public string CategoryId { get; set; }

        [Required, MaxLength(100)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public List<FileUploadModel> Images { get; set; }

        public string DistrictId { get; set; }

        public string DivisionId { get; set; }

        [Required]
        public string UserId { get; set; }

        public PublicMessageInsertModel()
        {

        }
    }
}
