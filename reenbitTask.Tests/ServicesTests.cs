using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Azure;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using reenbitTask.Services;

namespace reenbitTask.Tests
{
    [TestFixture]
    public class BlobServiceTests
    {
        [Test]
        public async Task UploadFileBlobASync_Should_Upload_File()
        {
            // Arrange
            var mockBlobServiceClient = new Mock<BlobServiceClient>();
            var mockBlobContainerClient = new Mock<BlobContainerClient>();
            var mockBlobClient = new Mock<BlobClient>();
            var mockFormFile = new Mock<IFormFile>();

            // Setup GetBlobContainerClient to return the mockBlobContainerClient
            mockBlobServiceClient.Setup(client => client.GetBlobContainerClient("docxfiles"))
                .Returns(mockBlobContainerClient.Object);

            // Setup GetBlobClient to return the mockBlobClient when called with any non-null string
            mockBlobContainerClient.Setup(client => client.GetBlobClient(It.IsNotNull<string>()))
                .Returns(mockBlobClient.Object);

            var blobService = new BlobService(mockBlobServiceClient.Object);

            // Act
            await blobService.UploadFileBlobASync("test@example.com", mockFormFile.Object);

            // Assert
            // Verify that GetBlobClient is called with any non-null string exactly once
            mockBlobContainerClient.Verify(client => client.GetBlobClient(It.IsNotNull<string>()), Times.Once);
        }




        [Test]
        public async Task ListBlobsAsync_Should_Return_List_Of_Blob_Names()
        {
            // Arrange
            var mockBlobServiceClient = new Mock<BlobServiceClient>();
            var mockBlobContainerClient = new Mock<BlobContainerClient>();
            var mockBlobItem = new Mock<BlobItem>();

            // Setup GetBlobClient to return a mockBlobClient
            mockBlobContainerClient.Setup(client => client.GetBlobClient(It.IsAny<string>()))
                .Returns(new Mock<BlobClient>().Object);

            // Setup GetBlobsAsync to return an AsyncPageable<BlobItem>
            var blobList = new BlobItem[]
            {
            BlobsModelFactory.BlobItem("Blob1"),
            BlobsModelFactory.BlobItem("Blob2"),
            BlobsModelFactory.BlobItem("Blob3")
            };

            var page = Page<BlobItem>.FromValues(blobList, null, Mock.Of<Response>());
            var pageableBlobList = AsyncPageable<BlobItem>.FromPages(new[] { page });

            mockBlobContainerClient.Setup(client => client.GetBlobsAsync(It.IsAny<BlobTraits>(), It.IsAny<BlobStates>(), It.IsAny<string>(), It.IsAny<System.Threading.CancellationToken>()))
                .Returns(pageableBlobList);

            mockBlobServiceClient.Setup(client => client.GetBlobContainerClient("docxfiles"))
                .Returns(mockBlobContainerClient.Object);

            var blobService = new BlobService(mockBlobServiceClient.Object);

            // Act
            var result = await blobService.ListBlobsAsync();

            // Assert
            Assert.That(result != null);
            Assert.That(result.Count() == 3);
        }


    }
}
