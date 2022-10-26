using System;
using System.IO;

namespace EnterpriseApp.API.Data.ViewModels
{
    public class FileDownloadModel
    {
        public Stream Stream { get; set; }
        public FileStream FileStream { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
    }
}
