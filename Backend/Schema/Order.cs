namespace Schema
{
    public record Order {
        public int Id { get; init;}
        public int UserId { get; init; }
        public string OrderNumber { get; init;} = string.Empty;
        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; init; }

        public virtual ICollection<OrderDetail>? OrderDetails { get; init; }
    }
}