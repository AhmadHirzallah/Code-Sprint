namespace DTO
{
    /// <summary>
    /// Response model for PDF generation operations
    /// </summary>
    public class GenerationResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? FileName { get; set; }
        public DateTime ProcessedAt { get; set; }
        public string? Error { get; set; }
    }

    /// <summary>
    /// Response model for multiple file operations
    /// </summary>
    public class MultipleFilesGenerationResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int TotalFiles { get; set; }
        public int SuccessfulFiles { get; set; }
        public int FailedFiles { get; set; }
        public List<GenerationResponse> Results { get; set; } = new List<GenerationResponse>();
        public List<GenerationError> Errors { get; set; } = new List<GenerationError>();
        public DateTime ProcessedAt { get; set; }
    }

    /// <summary>
    /// Error model for file processing errors
    /// </summary>
    public class GenerationError
    {
        public string FileName { get; set; } = string.Empty;
        public string Error { get; set; } = string.Empty;
    }

    /// <summary>
    /// Request model for additional information
    /// </summary>
    public class GenerationRequest
    {
        public string? AdditionalInformation { get; set; }
    }
}