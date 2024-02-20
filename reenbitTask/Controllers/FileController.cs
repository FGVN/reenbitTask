using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using reenbitTask.Services;
using System;
using System.IO;

namespace reenbitTask.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileController : ControllerBase
    {
        private readonly ILogger<FileController> _logger;
        private readonly IBlobService _blobService;

        public FileController(ILogger<FileController> logger, IBlobService blobService)
        {
            _logger = logger;
            _blobService = blobService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> PostFile([FromForm] string email, [FromForm] IFormFile file)
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

                       await _blobService.UploadFileBlobASync(email, file);

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
