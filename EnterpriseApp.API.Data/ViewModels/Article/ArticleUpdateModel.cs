using System;
using System.ComponentModel.DataAnnotations;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class ArticleUpdateModel : ArticleInsertModel
    {
        [Required]
        public string Id { get; set; }
    }
}
