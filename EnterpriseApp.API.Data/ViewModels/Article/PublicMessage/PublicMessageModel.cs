using System;
using System.Collections.Generic;
using EnterpriseApp.API.Data.Entity;
using EnterpriseApp.API.Data.ViewModels;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class PublicMessageModel : BaseEntityModel
    {
        public string CategoryId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public List<FileUploadModel> Images { get; set; }

        public string DistrictId { get; set; }

        public string DivisionId { get; set; }

        public string UserId { get; set; }

        public UserModel Reporter { get; set; }

        public LocationModel Location { get; set; }

        public string ThumbUrl { get; set; }

        public List<UserCommentModel> UserComments { get; set; }

        public PublicMessageModel()
        {
            Images = new List<FileUploadModel>();

            UserComments = new List<UserCommentModel>();
        }
    }
}
