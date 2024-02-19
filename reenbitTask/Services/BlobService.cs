using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Net.Mime;

namespace reenbitTask.Services
{
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;

        public BlobService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public Task DeleteBlobAsync(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<BlobInfo> GetBlobAsync(string name)
        {
            //var containerClient = _blobServiceClient.GetBlobContainerClient("docxfiles");
            //var blobClient = containerClient.GetBlobClient(name);
            //var blobDonwloadInfo = await blobClient.DownloadAsync();
            //return new BlobInfo(blobDonwloadInfo.Value.Content, blobDonwloadInfo.Value.ContentType);
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<string>> ListBlobsAsync()
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient("docxfiles");
            var items = new List<string>();

            await foreach(var item in containerClient.GetBlobsAsync())
            {
                items.Add(item.Name);
                Console.WriteLine(item.Name);
            }

            return items;
        }
        public async Task UploadFileBlobASync(IFormFile file)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient("docxfiles");
            var blobClient = containerClient.GetBlobClient(file.FileName);

            var blobHttpHeaders = new BlobHttpHeaders
            {
                ContentType = file.ContentType
            };

            await blobClient.UploadAsync(file.OpenReadStream(), blobHttpHeaders);
        }
    }
}
