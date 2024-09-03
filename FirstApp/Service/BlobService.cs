using Azure.Storage.Blobs;

namespace FirstApp.Service
{
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;
        private readonly string _blobStorageConnectionString;
        private readonly IConfiguration _configuration;

        public BlobService(IConfiguration configuration)
        {
            _configuration = configuration;
            _blobStorageConnectionString = _configuration["BlobStorageConnectionString"];
            _containerName = _configuration["ContainerName"];
            _blobServiceClient = new BlobServiceClient(_blobStorageConnectionString);
        }

        public async Task UploadBlobAsync(string blobName, Stream data)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            await containerClient.CreateIfNotExistsAsync();

            var blobClient = containerClient.GetBlobClient(blobName);
            await blobClient.UploadAsync(data, overwrite: true);
        }

        public async Task<Stream> DownloadBlobAsync(string blobName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(blobName);

            var downloadResponse = await blobClient.DownloadAsync();
            return downloadResponse.Value.Content;
        }

        public async Task<bool> BlobExistsAsync(string blobName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(blobName);

            return await blobClient.ExistsAsync();
        }
    }
}