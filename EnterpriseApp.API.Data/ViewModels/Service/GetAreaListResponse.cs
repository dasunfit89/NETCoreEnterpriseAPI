using System;
using System.Collections.Generic;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class GetAreaListResponse
    {
        public IEnumerable<AreaModel> AreaList { get; set; }
    }
}
