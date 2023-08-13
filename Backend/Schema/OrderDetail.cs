namespace Schema
{
    public record OrderDetail {
        public int Id { get; init;}
        public int OrderId { get; init; }
        public int ProductId { get; init;}
        public int Quantity { get; init;}
        public double QuotedPrice { get; init;}
        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; init; }

        public virtual Order? Order {get;set;}
        public virtual Product? Product {get;set;}
    }
}