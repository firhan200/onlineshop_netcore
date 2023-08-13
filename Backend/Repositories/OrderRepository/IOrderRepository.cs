using Schema;

namespace Repositories.OrderRepository
{
    public interface IOrderRepository : IGenericRepository<Order> {
        List<Order> GetOrdersByUserId(int userId);
    }
}