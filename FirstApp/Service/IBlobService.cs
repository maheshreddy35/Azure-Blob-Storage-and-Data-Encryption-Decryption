namespace FirstApp.Service
{
    public interface IBlobService
    {
        public Task UploadBlobAsync(string blobName, Stream data);
        public Task<Stream> DownloadBlobAsync(string blobName);
        public Task<bool> BlobExistsAsync(string blobName);
    }
}
