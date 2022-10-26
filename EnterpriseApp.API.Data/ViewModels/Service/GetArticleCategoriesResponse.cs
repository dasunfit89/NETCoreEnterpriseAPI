using System;
using System.Collections.Generic;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class GetArticleCategoriesResponse
    {
        public IEnumerable<CategoryModel> CategoryList { get; set; }
    }
}
