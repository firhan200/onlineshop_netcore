using System.Security.Claims;
using DTOs.Cart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using Repositories.OrderRepository;
using Repositories.ProductRepository;
using Schema;

namespace Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    [Authorize(Roles = nameof(Enums.Roles.User))]
    public class OrdersController : ControllerBase { 
        private readonly IOrderRepository _orderRepository;
        private readonly IGenericRepository<OrderDetail> _orderDetailsRepository;
        private readonly IProductRepository _productRepository;

        public OrdersController(
            IOrderRepository orderRepository,
            IGenericRepository<OrderDetail> orderDetailsRepository,
            IProductRepository productRepository
        ){
            _orderRepository = orderRepository;
            _orderDetailsRepository = orderDetailsRepository;
            _productRepository = productRepository;
        }

        [HttpGet(Name = "Get Orders")]
        public IResult GetOrders(){
            int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.Sid));
            List<DTOs.Order.OrderDTO> results = new (); 

            //get order from DB
            List<Order> orders = _orderRepository.GetOrdersByUserId(userId);

            foreach(var order in orders){
                results.Add(new DTOs.Order.OrderDTO {
                    Id = order.Id,
                    OrderNumber = order.OrderNumber,
                    Details = order.OrderDetails?.Select(x => new DTOs.Order.OrderDetail
                    {
                        Id = x.Id,
                        ProductId = x.ProductId,
                        Quantity = x.Quantity,
                        QuotedPrice = x.QuotedPrice,
                        Product = x.Product
                    }).ToList()
                });
            }

            return Results.Ok(results);
        }

        [HttpPost(Name = "Create Orders")]
        public IResult CreateOrderFromCart([FromBody]List<CartDTO> cart){
             //get product ids
            List<int> productIds = cart.Select(x => x.ProductId).ToList();

            //get products detail
            var products = _productRepository.GetProductsByIds(productIds);

            //insert order
            Order? order = _orderRepository.Create(new Order{
                UserId = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid)?.Value),
                OrderNumber = Guid.NewGuid().ToString(),
            });

            if(order is null){
                return Results.BadRequest();
            }

            //insert order details
            foreach(var cartItem in cart){
                //get product quoted price
                double price = products.Where(x => x.Id == cartItem.ProductId).FirstOrDefault()?.Price ?? 0;
                _orderDetailsRepository.Create(new OrderDetail{
                    OrderId = order.Id,
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    QuotedPrice = price
                });
            }

            return Results.Ok();
        }
    }
}