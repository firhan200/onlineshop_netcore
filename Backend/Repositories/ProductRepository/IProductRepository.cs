using Schema;

namespace Repositories.ProductRepository
{
    public interface IProductRepository{
        List<Product> GetProductsByIds(List<int> productIds);
    }
}