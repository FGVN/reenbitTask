using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Moq;
using reenbitTask.Controllers;
using reenbitTask.Services;
using System.IO;
using System.Threading.Tasks;

namespace reenbitTask.Tests
{
    [TestFixture]
    public class ControllerTests
    {
        private Mock<ILogger<FileController>> _mockLogger;
        private Mock<IBlobService> _mockBlobService;

        [SetUp]
        public void Setup()
        {
            _mockLogger = new Mock<ILogger<FileController>>();
            _mockBlobService = new Mock<IBlobService>();
        }

        [Test]
        public async Task PostFile_ValidInput_ReturnsOkResult()
        {
            // Arrange
            var controller = new FileController(_mockLogger.Object, _mockBlobService.Object);

            var formFile = new Mock<IFormFile>();
            formFile.Setup(f => f.FileName).Returns("test.docx");
            formFile.Setup(f => f.Length).Returns(1);

            // Act
            var result = await controller.PostFile("test@gmail.com", formFile.Object);

            // Assert
            var okResult = (OkObjectResult)result;

            // Extract the "message" property and compare
            Assert.That(okResult.Value.ToString(), Is.EqualTo(("{ message = File uploaded successfully. }")));
        }


        [Test]
        public async Task PostFile_InvalidEmail_ReturnsBadRequest()
        {
            // Arrange
            var controller = new FileController(_mockLogger.Object, _mockBlobService.Object);

            // Act
            var result = await controller.PostFile("invalidemail", null);

            var badRequestResult = (BadRequestObjectResult)result;
            Assert.That(badRequestResult.Value.ToString(), Is.EqualTo("{ message = Invalid email address. }"), "But was: " + badRequestResult.Value);
        }

        [Test]
        public async Task PostFile_NullFormFile_ReturnsBadRequest()
        {
            // Arrange
            var controller = new FileController(_mockLogger.Object, _mockBlobService.Object);

            // Act
            var result = await controller.PostFile("test@gmail.com", null);

            // Assert
            var badRequestResult = (BadRequestObjectResult)result;
            Assert.That(badRequestResult.Value.ToString(), Is.EqualTo("{ message = No file provided. }"), "But was: " + badRequestResult.Value);
        }

        [Test]
        public async Task PostFile_InvalidFileFormat_ReturnsBadRequest()
        {
            // Arrange
            var controller = new FileController(_mockLogger.Object, _mockBlobService.Object);

            var formFile = new Mock<IFormFile>();
            formFile.Setup(f => f.FileName).Returns("test.txt");
            formFile.Setup(f => f.Length).Returns(1);

            // Act
            var result = await controller.PostFile("test@gmail.com", formFile.Object);

            // Assert
            var badRequestResult = (BadRequestObjectResult)result;
            Assert.That(badRequestResult.Value.ToString(), Is.EqualTo("{ message = Invalid file format. Only .docx files are allowed. }"), "But was: " + badRequestResult.Value);
        }


    }
}
