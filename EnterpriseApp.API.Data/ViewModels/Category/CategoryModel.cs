using System;
using System.Collections.Generic;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class CategoryModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public List<SubCategoryModel> SubCategories { get; set; }
    }
}
