namespace Schema
{
    public class Image {
        public int Id { get; set;}
        public string UrlPath { get; set; } = string.Empty;
        public int Size { get; set; }
        public string Extension { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}