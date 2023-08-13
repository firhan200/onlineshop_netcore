namespace DTOs.Products
{
    public class AddEditProductDTO{
        public string PhotoUrl { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Price { get; set; } = 0;
    }
}