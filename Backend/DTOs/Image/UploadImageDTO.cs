namespace DTOs.Image
{
    public class UploadImageDTO {
        public bool Success { get; set; }
        public string? UrlPath { get; set; }
        public string? ErrorMessage { get; set; }
    }
}