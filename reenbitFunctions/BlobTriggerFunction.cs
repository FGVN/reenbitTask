using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;
using System;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;

public static class BlobTriggerFunction
{
    [FunctionName("BlobTriggerFunction")]
    public static void Run(
        [BlobTrigger("docxfiles/{email}/{name}", Connection = "AzureWebJobsStorage")] Stream myBlob,
        string email,
        string name,
        ILogger log,
        ExecutionContext context)
    {
        log.LogInformation($"C# Blob trigger function processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");

        var config = new ConfigurationBuilder()
            .SetBasePath(context.FunctionAppDirectory)
            .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        var sasToken = GenerateSasToken(
            config.GetValue<string>("AzureWebJobsStorage"), "docxfiles", $"{email}/{ name}");

        Console.WriteLine($"File info of file {name} is sent to {email}");
        // Send email notification
        var blobUrl = sasToken;

        SendEmail(email, $@"
                <html>
                    <body>
                        <p>Hello,</p>
                        <p>Your file has been successfully uploaded.</p>
                        <p>You can download it using the following link:</p>
                        <p><a href='{blobUrl}'>{blobUrl}</a></p>
                    </body>
                </html>",
                config.GetValue<string>("EmailSender"),
                config.GetValue<string>("SenderPassword"));
    }

    private static void SendEmail(string email, string body, string sender, string senderPass)
    {
        using (var client = new SmtpClient("smtp.gmail.com", 587))
        {
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(sender, senderPass);
            client.EnableSsl = true;

            using (var message = new MailMessage(sender, email, "Hi, this is email from Ivan Holovai", body))
            {
                message.IsBodyHtml = true;
                client.Send(message);
            }
        }
        System.Console.WriteLine("Sent");

    }
    private static string GenerateSasToken(string storageConnectionString, string containerName, string blobName)
    {
        var blobServiceClient = new BlobServiceClient(storageConnectionString);
        var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(blobName);

        if (!blobClient.CanGenerateSasUri)
        {
            throw new InvalidOperationException("BlobClient object has not been authorized to generate shared key credentials. " +
                "Verify --azure-storage-connection-key is valid and has proper permissions.");
        }

        var sasBuilder = new BlobSasBuilder
        {
            BlobContainerName = containerClient.Name,
            BlobName = blobClient.Name,
            Resource = "bsco"
        };

        sasBuilder.StartsOn = DateTimeOffset.UtcNow.AddMinutes(-5);
        sasBuilder.ExpiresOn = DateTimeOffset.UtcNow.AddHours(1);

        sasBuilder.SetPermissions(BlobSasPermissions.Read);

        var token = blobClient.GenerateSasUri(sasBuilder);

        return token.ToString();
    }


}
