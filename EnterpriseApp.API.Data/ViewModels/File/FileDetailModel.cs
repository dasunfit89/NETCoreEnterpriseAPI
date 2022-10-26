using System;
using EnterpriseApp.API.Data.ViewModels;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class FileDetailModel
    {
        public CommonFileUploadModel File { get; set; }

        public bool IsSuccessful { get; set; }
    }
}
