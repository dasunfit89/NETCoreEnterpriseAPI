using System;
using System.Collections.Generic;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class GetAppUsersResponse
    {
        public List<LightUserModel> Items { get; set; }

        public GetAppUsersResponse()
        {
        }
    }
}
