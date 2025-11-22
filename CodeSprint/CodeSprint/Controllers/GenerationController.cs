using Microsoft.AspNetCore.Mvc;
using BL;

namespace CodeSprint.Controllers
{
    [Route("api/Generation")]
    [ApiController] // application/
    public class GenerationController : ControllerBase
    {
        private readonly ILogger<GenerationController> _logger;
        private readonly CodeGenerator _codeGenerator;
        private const int MaxFileSizeInBytes = 20 * 1024 * 1024; // 20 MB
        private readonly string[] AllowedExtensions = {
                                                            ".pdf", ".png", ".jpg", ".jpeg",
                                                            ".bmp", ".tiff", ".tif", ".webp",
                                                    };
        public GenerationController(ILogger<GenerationController> logger)
        {
            _logger = logger;
            _codeGenerator = new CodeGenerator();
        }

        /// <summary>
        /// Process one or multiple PDF files to extract multiple-choice questions
        /// Returns JSON array of questions directly from AI processing
        /// </summary>
        /// <param name="files">Single file or list of PDF files to process</param>
        /// <param name="additionalInformation">Optional additional instructions for AI processing</param>
        /// <returns>JSON array of extracted questions from all files combined</returns>
        [HttpPost("ProcessFiles")]
        public async Task<IActionResult> ProcessFiles(
            [FromForm] List<IFormFile> files,
            [FromForm] string? additionalInformation = "")
        {
            try
            {
                // Validate that files are provided
                if (files == null || files.Count == 0)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "No files provided"
                    });
                }

                _logger.LogInformation("Processing {FileCount} PDF file(s)", files.Count);

                var allQuestions = new List<object>();
                var processedFiles = new List<string>();
                var failedFiles = new List<object>();
                string aiResponse = "";


                foreach (var file in files)
                {
                    try
                    {
                        // Validate each file
                        var validationResult = ValidateFile(file);
                        if (validationResult != null)
                        {
                            failedFiles.Add(new
                            {
                                fileName = file.FileName,
                                error = "File validation failed"
                            });
                            continue;
                        }

                        _logger.LogInformation("Processing file: {FileName}", file.FileName);

                        // Save file temporarily
                        string tempFilePath = await SaveFileTemporarily(file);
                        // Call BL method and get JSON response
                        aiResponse = await _codeGenerator.GenerateFromOneFileAsync(tempFilePath, additionalInformation ?? "");



                        processedFiles.Add(file.FileName);
                        _logger.LogInformation("Successfully processed file: {FileName}", file.FileName);
                    }
                    catch (Exception fileEx)
                    {
                        _logger.LogError(fileEx, "Error processing file: {FileName}", file.FileName);
                        failedFiles.Add(new
                        {
                            fileName = file.FileName,
                            error = fileEx.Message
                        });
                    }
                }



                return Ok(new
                {
                    aiResponse
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing PDF files");
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while processing the PDF files",
                    error = ex.Message
                });
            }
        }

        #region Private Helper Methods

        /// <summary>
        /// Validates uploaded file for type, size, and extension
        /// </summary>
        /// <param name="file">File to validate</param>
        /// <returns>BadRequest result if validation fails, null if valid</returns>
        private IActionResult? ValidateFile(IFormFile? file)
        {
            if (file == null)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "No file provided"
                });
            }

            if (file.Length == 0)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "File is empty"
                });
            }

            if (file.Length > MaxFileSizeInBytes)
            {
                return BadRequest(new
                {
                    success = false,
                    message = $"File size exceeds maximum allowed size of {MaxFileSizeInBytes / (1024 * 1024)}MB"
                });
            }

            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(fileExtension))
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Only PDF files are allowed"
                });
            }

            // Additional MIME type validation
            if (!file.ContentType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Invalid file type. Only PDF files are accepted"
                });
            }

            return null;
        }

        /// <summary>
        /// Saves uploaded file to a temporary location
        /// </summary>
        /// <param name="file">File to save</param>
        /// <returns>Temporary file path</returns>
        private async Task<string> SaveFileTemporarily(IFormFile file)
        {
            // Create temp directory if it doesn't exist
            var tempDir = Path.Combine(Path.GetTempPath(), "PDFProcessing");
            if (!Directory.Exists(tempDir))
            {
                Directory.CreateDirectory(tempDir);
            }

            // Generate unique temp file name
            var tempFileName = $"{Guid.NewGuid()}_{file.FileName}";
            var tempFilePath = Path.Combine(tempDir, tempFileName);

            // Save file to temp location
            using (var stream = new FileStream(tempFilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            _logger.LogDebug("Saved temporary file: {TempFilePath}", tempFilePath);
            return tempFilePath;
        }

        #endregion

        // Keep existing methods for backward compatibility
        // GET: api/Generation
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/Generation/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/Generation
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<GenerationController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<GenerationController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}