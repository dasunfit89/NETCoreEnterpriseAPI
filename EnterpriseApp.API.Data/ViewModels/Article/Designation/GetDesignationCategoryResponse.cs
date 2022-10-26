using System;
using System.Collections.Generic;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class GetDesignationCategoryResponse : BaseResponse
    {
        public List<DesignationCategoryModel> Items { get; set; }
    }
}
