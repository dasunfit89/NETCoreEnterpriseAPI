using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EnterpriseApp.API.Data.Entity;
using EnterpriseApp.API.Data.ViewModels;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class PartyDesignationModel : BaseEntityModel
    {
        public string DesignationId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<FileUploadModel> Images { get; set; }

        public string DistrictId { get; set; }

        public string DivisionId { get; set; }

        public DesignationCategoryModel Designation { get; set; }

        public string ThumbUrl { get; set; }

        public int Order { get; set; }

        public PartyDesignationModel()
        {
            Images = new List<FileUploadModel>();
        }
    }
}

