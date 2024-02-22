# Reenbit Task

Net Web App with Angular UI to send docx to blob and receive email notification using Azure Function with SAS token

## Prerequisites

- [.NET 6](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Node.js](https://nodejs.org/)
- [Angular CLI](https://cli.angular.io/)

## Installation

1. Clone the repository.
2. Open terminal/command prompt and navigate to the `reenbitFunctions` folder.
3. Run the following command to restore dependencies and build the .NET project: 

    ```bash
   dotnet build
   ```
5. Navigate to the reenbitTask folder.
6. Copy the provided appsettings.json file into the root of the reenbitTask project.
7. Run the following command to restore dependencies and build the .NET project:
  ```bash
  dotnet build
  ```
7. Navigate to the reenbitTask.ClientApp folder.
Run the following command to restore Node.js dependencies and build the Angular project:
  ```bash
  npm install
  ```
# Configuration
## reenbitFunctions/local.settings.json
```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "YOUR_AZURE_STORAGE_CONNECTION_STRING",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "EmailSender": "YOUR_EMAIL_SENDER",
    "SenderPassword": "YOUR_SENDER_PASSWORD"
  }
}
```
## reenbitTask/appsettings.json

```json
{
  "AzureBlobStorageConnectionString":"YOUR_BLOB_CONNECTION_STRING"
   "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*"
  }
```
# How to Run
## Navigate to the reenbitFunctions folder and run the following command:
  ```bash
  dotnet run
  ```
## Open a new terminal window, navigate to the reenbitTask folder, and run the following command:
  ``` bash
  dotnet run
  ```
## Open another terminal window, and navigate to the reenbitTask.Client folder, and run the following command:
  ``` bash
  ng serve
  ```
## Open your browser and visit http://localhost:4200 to interact with the Angular app.

# Testing
## Run the following command in the respective project:

For Unit Tests in reenbitTask.Tests:
```bash
dotnet test
```

# API Documentation
API has only 1 endpoint 

```https
/file/upload/{email}/{file}
```

Returns IActionResult with BadRequest or Ok and message 

## BadRequest has the following messages:

Invalid email address

No file provided

Invalid file format. Only .docx files are allowed.

## Ok returns the following:
File uploaded successfully.
