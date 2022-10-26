using System;
namespace EnterpriseApp.API.Data.ViewModels
{
    public class GetArticleListRequest
    {
        public string UserId { get; set; }

        public string DistrictId { get; set; }

        public int Status { get; set; }

        public bool IsAdmin { get; set; }

        public GetArticleListRequest()
        {
        }
    }
}
