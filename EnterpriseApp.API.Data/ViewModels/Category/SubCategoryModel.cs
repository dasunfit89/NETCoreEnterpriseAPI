using System;
using System.Collections.Generic;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class SubCategoryModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public List<CategoryPropModel> Props { get; set; }

        public List<ArticleTagModel> Tags { get; set; }
    }
}
