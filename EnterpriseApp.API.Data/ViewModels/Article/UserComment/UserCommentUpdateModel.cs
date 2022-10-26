using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EnterpriseApp.API.Data.Entity;
using EnterpriseApp.API.Data.ViewModels;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class UserCommentUpdateModel
    {
        [Required]
        public string ArticleId { get; set; }

        [Required]
        public string Id { get; set; }

        [Required]
        public string ArticleType { get; set; }

        public UserCommentUpdateModel()
        {

        }
    }
}
