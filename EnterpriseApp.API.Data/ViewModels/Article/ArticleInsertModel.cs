using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EnterpriseApp.API.Data.Entity;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class ArticleInsertModel
    {
        [Required, MaxLength(100)]
        public string Title { get; set; }

        [Required, MaxLength(100)]
        public string AdminArea { get; set; }

        [Required, MaxLength(100)]
        public string Street { get; set; }

        [Required, MaxLength(100)]
        public string Locality { get; set; }

        [Required, MaxLength(100)]
        public string HouseNo { get; set; }

        [Required, MaxLength(1000)]
        public string Description { get; set; }

        [Required, MaxLength(100)]
        public string Address { get; set; }

        [Required]
        public Coordinate Coordinates { get; set; }

        [Required]
        public string CategoryId { get; set; }

        [Required]
        public string SubCategoryId { get; set; }

        [Required]
        public ArticlePriceModel Price { get; set; }

        [Required]
        public List<FileUploadModel> Images { get; set; }

        [Required]
        public List<ArticleTagModel> Tags { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}
