using Schema;

namespace DTOs.Cart
{
    public class CartDetailDTO {
        public int Quantity { get; set; }
        public Product? Product { get; set; }
    }
}