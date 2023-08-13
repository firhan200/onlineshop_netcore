namespace Schema
{
    public class Authentication {
        public int Id { get; set;}
        public int UserId { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ExpiresAt { get; set; }

        public virtual User? User { get; set; }
    }
}