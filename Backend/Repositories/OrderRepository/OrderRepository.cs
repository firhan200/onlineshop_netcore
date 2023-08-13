using Data;
using Microsoft.EntityFrameworkCore;
using Schema;

namespace Repositories.OrderRepository
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly DbSet<Order> _orders;

        public OrderRepository(DataContext dataContext) : base(dataContext)
        {
            _orders = dataContext.Set<Order>();
        }

        public List<Order> GetOrdersByUserId(int userId)
        {
            #nullable disable
            return _orders
                .Include(x => x.OrderDetails)
                .ThenInclude(n => n.Product)
                .Where(x => x.UserId == userId)
                .ToList();
            #nullable restore
        }
    }
}