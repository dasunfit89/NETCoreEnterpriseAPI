using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EnterpriseApp.API.Data.Entity;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class DeleteArticleRequest
    {
        [Required]
        public string ArticleId { get; set; }

        [Required]
        public string ArticleType { get; set; }

        public int Status { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}
