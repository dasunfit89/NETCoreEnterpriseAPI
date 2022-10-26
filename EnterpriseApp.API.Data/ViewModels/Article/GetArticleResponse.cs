using System;
namespace EnterpriseApp.API.Data.ViewModels
{
    public class GetArticleResponse : BaseResponse
    {
        public ArticleModel Article { get; set; }
    }
}
