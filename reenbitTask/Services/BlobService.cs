using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace reenbitTask.Services
{
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string containername = "docxfiles";

        public BlobService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }
        public async Task<IEnumerable<string>> ListBlobsAsync()
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containername);
            var items = new List<string>();

            await foreach(var item in containerClient.GetBlobsAsync())
            {
                items.Add(item.Name);
                Console.WriteLine(item.Name);
            }

            return items;
        }
        public async Task UploadFileBlobASync(string email, IFormFile file)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containername);

            var blobName = $"{email}/{file.FileName}";
            var blobClient = containerClient.GetBlobClient(blobName);

            var blobHttpHeaders = new BlobHttpHeaders
            {
                ContentType = file.ContentType
            };

            await blobClient.UploadAsync(file.OpenReadStream(), blobHttpHeaders);
        }
    }
}
