using NUnit.Framework;
using System.Net.Mail;

namespace reenbitFunctions.Tests
{
    [TestFixture]
    public class BlobTriggerFunctionTests
    {
        [Test]
        public void SendEmail_Should_Throw_Exception_On_Incorrect_Credentials()
        {
            // Arrange
            var email = "test@example.com";
            var body = "test body";
            var senderPass = "incorrect_password";
            var sender = "from@example.com";


            var wrong_sender = "your_email_sender";


            // Act & Assert
            // Mail format exception for sender and recepient
            Assert.Throws<FormatException>(() => BlobTriggerFunction.SendEmail(email, body, wrong_sender, senderPass));
            Assert.Throws<FormatException>(() => BlobTriggerFunction.SendEmail(wrong_sender, body, email, senderPass));

            // Smtp exception with wrong credentials
            Assert.Throws<SmtpException>(() => BlobTriggerFunction.SendEmail(email, body, sender, senderPass));
        }

        [Test]
        public void GenerateSasToken_Should_Throw_Exception_On_Invalid_SAS()
        {
            // Arrange
            var incorrectFormatConnectionString = "your_invalid_storage_connection_string";

            var correctFormatConnectionString =
                "DefaultEndpointsProtocol=https;AccountName=youraccountname;AccountKey=youraccountkey;EndpointSuffix=core.windows.net\r\n";
            var containerName = "docxfiles";
            var blobName = "test@example.com/file.txt";

            // Act & Assert
            // Format exception if connection string is incorrectly formated 
            Assert.Throws<FormatException>(() => BlobTriggerFunction.GenerateSasToken(incorrectFormatConnectionString, containerName, blobName));

            // Format exception if connection string is correctly formated 
            Assert.Throws<FormatException>(() => BlobTriggerFunction.GenerateSasToken(correctFormatConnectionString, containerName, blobName));
        }

    }
}
