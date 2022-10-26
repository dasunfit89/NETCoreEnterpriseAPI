using System;
using System.ComponentModel.DataAnnotations;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class ArticleTagModel
    {
        [Required]
        public string Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }

        [MaxLength(50)]
        public string Icon { get; set; }

        [Required, MaxLength(100)]
        public string Value { get; set; }
    }
}
