using Cft.Fbo.Domain.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using System.Threading.Tasks;

namespace EnterpriseApp.API.Core.Services
{
    public class FileSystemService : IFileSystemService
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public AzureBlobService(ILogger<FileSystemService> logger,
          IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public string CreateUserDirectory(string directoryId, string folderPath)
        {
            string directoryPathTo = Path.Combine(folderPath, directoryId);

            if (!Directory.Exists(directoryPathTo) && folderPath != null)
                Directory.CreateDirectory(directoryPathTo);

            return directoryPathTo;
        }

        public async Task<bool> DeleteFileAsync(FileOperationModel fileModel, string filePath)
        {
            CloudBlockBlob blockBlob = await InitializeSettings(fileModel);

            bool exists = await blockBlob.ExistsAsync();

            if (exists)
            {
                await blockBlob.DeleteAsync();
                return await Task.FromResult(true);
            }
            else
            {
                return await Task.FromResult(false);
            }
        }

        public async Task<FileDownloadModel> DownloadFileAsync(FileOperationModel fileModel, string filePath)
        {
            FileDownloadModel model = new FileDownloadModel();

            CloudBlockBlob blockBlob = await InitializeSettings(fileModel);

            bool exists = await blockBlob.ExistsAsync();

            if (!exists)
                return model;

            Stream blobStream = await blockBlob.OpenReadAsync();

            model.Stream = blobStream;
            model.ContentType = blockBlob.Properties.ContentType;

            return model;
        }

        public async Task<bool> SaveFileAsync(FileOperationModel fileModel, string localFileName, string directoryPath)
        {
            if (string.IsNullOrEmpty(fileModel.ActualFileName))
                return false;

            CloudBlockBlob blockBlob = await InitializeSettings(fileModel);
            if (blockBlob == null)
                return false;

            // Create or overwrite the created blob with contents from a local file.
            using (var fileStream = System.IO.File.OpenRead(localFileName))
            {
                await blockBlob.UploadFromStreamAsync(fileStream);
            }

            File.Delete(localFileName);

            return true;
        }

        private async Task<CloudBlockBlob> InitializeSettings(FileOperationModel fileModel)
        {
            CloudBlockBlob blockBlob = null;
            string azureStorageConnStr = _configuration.GetSection("Application:Storage:AzureStorageConnectionString").Value;

            if (string.IsNullOrEmpty(azureStorageConnStr))
            {
                _logger.LogError("Azure Storage Settings Not Found.");
                return blockBlob;
            }

            // Parse the connection string and return a reference to the storage account.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(azureStorageConnStr);

            // Create blob service client
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve a reference to a container.
            CloudBlobContainer container = blobClient.GetContainerReference(fileModel.ContainerName);

            // Create the container if it doesn't already exist.
            bool isCreated = await container.CreateIfNotExistsAsync();

            // Set container permission as public (By default it is private)
            await container.SetPermissionsAsync(
               new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            // Retrieve reference to a blob named in containerName.
            blockBlob = container.GetBlockBlobReference(Path.Combine(fileModel.DirectoryId, fileModel.ActualFileName));

            _logger.LogInformation("Azure Storage Settings successfull. Account Name: {0} Container Name: {1} IsCreated: {2}", storageAccount.Credentials.AccountName, container.Name, isCreated);

            return blockBlob;
        }
    }
}
