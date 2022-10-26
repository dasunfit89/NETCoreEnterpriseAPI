using System;
using System.Collections.Generic;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class GetInformationCategoriesResponse : BaseResponse
    {
        public List<InformationCategoryModel> Items { get; set; }
    }
}
