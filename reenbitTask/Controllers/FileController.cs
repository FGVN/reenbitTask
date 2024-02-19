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
                if (email != string.Empty && IsValidEmail(email))
                {
                    // Check if a file is provided
                    if (file != null && file.Length > 0)
                    {
                        // Check if the file has a .docx extension
                        var fileName = Path.GetFileName(file.FileName);
                        if (!fileName.EndsWith(".docx", StringComparison.OrdinalIgnoreCase))
                        {
                            return BadRequest(new { message = "Invalid file format. Only .docx files are allowed." });
                        }

                        // Process the file
                        // You can now save the file, perform further processing, or store in Azure Blob Storage, etc.

                        _logger.LogInformation($"File '{fileName}' uploaded successfully for email '{email}'.");

                        return Ok(new { message = "File uploaded successfully." });
                    }
                    else
                    {
                        return BadRequest(new { message = "No file provided." });
                    }
                }
                else
                {
                    return BadRequest(new { message = "Invalid email address." });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error uploading file: {ex.Message}");
                return StatusCode(500, new { message = "Internal server error" });
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
