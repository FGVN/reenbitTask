using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace reenbitTask.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileController : ControllerBase
    {
        private readonly ILogger<FileController> _logger;

        public FileController(ILogger<FileController> logger)
        {
            _logger = logger;
        }

        [HttpPost("upload")]
        public IActionResult PostFile([FromForm] string email, [FromForm] IFormFile file)
        {
            try
            {
                // Check if the email is valid (you may implement additional validation)
                if (IsValidEmail(email))
                {
                    // Check if a file is provided
                    if (file != null && file.Length > 0)
                    {
                        // Process the file
                        var fileName = Path.GetFileName(file.FileName);
                        // You can now save the file, perform further processing, or store in Azure Blob Storage, etc.

                        _logger.LogInformation($"File '{fileName}' uploaded successfully for email '{email}'.");

                        return Ok("File uploaded successfully.");
                    }
                    else
                    {
                        return BadRequest("No file provided.");
                    }
                }
                else
                {
                    return BadRequest("Invalid email address.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error uploading file: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var mailAddress = new System.Net.Mail.MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
