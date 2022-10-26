using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using EnterpriseApp.API.Core.Exceptions;
using EnterpriseApp.API.Data;
using EnterpriseApp.API.Data.Entity;
using EnterpriseApp.API.Data.ViewModels;
using Microsoft.Extensions.Options;
using ApplicationDataException = EnterpriseApp.API.Core.Exceptions.ApplicationDataException;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Drawing;
using EnterpriseApp.API.Core.Extensions;

namespace EnterpriseApp.API.Core.Services
{
    public interface IFileService
    {
        Task<FileUploadModel> SaveFileAsync(FileUploadModel fileModel, bool isThumbnail = false);
        Task<FileDownloadModel> DownloadFileAsync(string fileId);
    }

    public class FileService : BaseService, IFileService
    {
        public FileService(IRepository dbContext, IMapper mapper, IOptions<AppSettings> appSettings) :
            base(dbContext, mapper, appSettings)
        {

        }

        public async Task<FileDownloadModel> DownloadFileAsync(string fileId)
        {
            FileDownloadModel fileDownloadModel = null;

            BlobClient blobClient = InitializeSettings(fileId);

            bool isExists = await blobClient.ExistsAsync();

            if (isExists)
            {
                BlobProperties properties = await blobClient.GetPropertiesAsync();

                Stream blobStream = await blobClient.OpenReadAsync();

                fileDownloadModel = new FileDownloadModel()
                {
                    Stream = blobStream,
                    FileName = fileId,
                    ContentType = properties.ContentType,
                };
            }

            return fileDownloadModel;
        }

        public async Task<FileUploadModel> SaveFileAsync(FileUploadModel fileModel, bool isThumbnail = false)
        {
            string fileId = Guid.NewGuid().ToString();

            fileModel.Filename = $"{fileId}{fileModel.Type}";

            BlobClient blobClient = InitializeSettings(fileModel.Filename);

            if (blobClient != null)
            {
                byte[] bytes = null;

                if (isThumbnail)
                {
                    //var resized = fileModel.Bytes.ResizeBase64(200d, 150d);
                    //bytes = Convert.FromBase64String(resized);
                    bytes = Convert.FromBase64String(fileModel.Bytes);
                }
                else
                {
                    bytes = Convert.FromBase64String(fileModel.Bytes);
                }

                using (var stream = new MemoryStream(bytes))
                {
                    var response = await blobClient.UploadAsync(stream, true);

                    fileModel.Url = $"file/images/{fileModel.Filename}";
                }
            }

            return fileModel;
        }

        private BlobClient InitializeSettings(string fileId)
        {
            string azureStorageConnStr = _appSettings.AzureStorageConnectionString;

            BlobServiceClient blobServiceClient = new BlobServiceClient(azureStorageConnStr);

            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_appSettings.FileUploadContainerName);

            BlobClient blobClient = containerClient.GetBlobClient(fileId);

            return blobClient;
        }
    }
}
