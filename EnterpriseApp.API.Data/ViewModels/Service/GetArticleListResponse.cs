using System;
using System.Collections.Generic;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class GetArticleListResponse
    {
        public IEnumerable<ArticleModel> ArticleList { get; set; }

        public MapExtentModel Extent { get; set; }
    }
}
