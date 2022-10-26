using System;
using System.Collections.Generic;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class ConfigUpdateResponse : BaseResponse
    {
        public ConfigurationModel Item { get; set; }
    }
}
