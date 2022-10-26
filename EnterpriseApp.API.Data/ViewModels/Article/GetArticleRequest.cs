using System;
using System.ComponentModel.DataAnnotations;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class GetArticleRequest
    {
        [Required]
        public string ArticleId { get; set; }
    }
}
