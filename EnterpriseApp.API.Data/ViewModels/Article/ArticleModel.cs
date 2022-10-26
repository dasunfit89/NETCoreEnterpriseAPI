using System;
using System.Collections.Generic;
using EnterpriseApp.API.Data.Entity;
using EnterpriseApp.API.Data.ViewModels;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class ArticleModel: BaseEntityModel
    {
        public string Title { get; set; }

        public string AdminArea { get; set; }

        public string Street { get; set; }

        public string Locality { get; set; }

        public string HouseNo { get; set; }

        public string Description { get; set; }

        public string Address { get; set; }

        public Coordinate Coordinates { get; set; }

        public string CategoryId { get; set; }

        public string SubCategoryId { get; set; }

        public CategoryModel Category { get; set; }

        public ArticlePriceModel Price { get; set; }

        public List<FileUploadModel> Images { get; set; }

        public List<ArticleTagModel> Tags { get; set; }

        public List<CategoryPropModel> Properties { get; set; }

        public string UserId { get; set; }

        public UserModel Reporter { get; set; }

        public ArticleModel()
        {
            Properties = new List<CategoryPropModel>();
        }
    }
}
