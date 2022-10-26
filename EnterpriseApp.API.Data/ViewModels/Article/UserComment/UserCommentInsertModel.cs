using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EnterpriseApp.API.Data.Entity;
using EnterpriseApp.API.Data.ViewModels;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class UserCommentInsertModel
    {
        [Required]
        public string ArticleId { get; set; }

        public string Id { get; set; }

        [Required]
        public string ArticleType { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required, MaxLength(1024)]
        public string Comment { get; set; }

        public UserCommentInsertModel()
        {

        }
    }
}
