using System;
using System.Collections.Generic;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class GetConfigurationResponse : BaseResponse
    {
        public ConfigurationModel AppConfig { get; set; }
    }
}
