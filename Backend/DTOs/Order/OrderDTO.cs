using Schema;

namespace DTOs.Order
{
    public record OrderDTO {
        public int Id { get; set;}
        public string OrderNumber { get; set;} = string.Empty;
        public List<OrderDetail>? Details{ get; set;}
    }

    public record OrderDetail {
        public int Id { get; set;}
        public int ProductId { get; set;}
        public double QuotedPrice { get; set;}
        public int Quantity { get; set;}
        public Product? Product { get; set;}
    }
}