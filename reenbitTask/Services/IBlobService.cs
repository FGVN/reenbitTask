using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace reenbitTask.Services
{
    public interface IBlobService
    {
        public Task<IEnumerable<string>> ListBlobsAsync();
        public Task UploadFileBlobASync(string email, IFormFile file);

        //Deleting and getting blob is not required fo the task

        // public Task DeleteBlobAsync(string name);
        // public Task<BlobInfo> GetBlobAsync(string name);
    }
}
