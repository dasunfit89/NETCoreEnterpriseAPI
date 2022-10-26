using System;
namespace EnterpriseApp.API.Data.ViewModels
{
    public class FileOperationModel
    {
        public string ContainerName { get; set; }
        public string ActualFileName { get; set; }
        public string TempPath { get; set; }
    }
}
