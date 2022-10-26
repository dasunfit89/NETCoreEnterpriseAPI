using System;
namespace EnterpriseApp.API.Data
{
    public class AppSettings
    {
        public string Secret { get; set; }

        public string Issuer { get; set; }

        public string APIRootPath { get; set; }

        public string AzureStorageConnectionString { get; set; }

        public string FileUploadContainerName { get; set; }

        public AppSettings()
        {

        }
    }
}
