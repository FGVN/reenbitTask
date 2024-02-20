using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace reenbitTask.Services
{
    public interface IBlobService
    {
        public Task<BlobInfo> GetBlobAsync(string name);
        public Task<IEnumerable<string>> ListBlobsAsync();
        public Task UploadFileBlobASync(string email, IFormFile file);
        public Task DeleteBlobAsync(string name);
    }
}
