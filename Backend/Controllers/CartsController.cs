using DTOs.Cart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.ProductRepository;

namespace Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("Api/[controller]")]
    public class CartsController : ControllerBase {
        private readonly IProductRepository _productRepository;

        public CartsController(IProductRepository productRepository){
            _productRepository = productRepository;
        }

        [HttpPost(Name = "Get Cart Product Detail")]
        public IResult GetCart([FromBody]List<CartDTO> cart){
            //get product ids
            List<int> productIds = cart.Select(x => x.ProductId).ToList();

            //get products detail
            var products = _productRepository.GetProductsByIds(productIds);

            List<CartDetailDTO> cartDetail = new List<CartDetailDTO>();
            foreach(var cartItem in cart){
                cartDetail.Add(new CartDetailDTO{
                    Product = products.FirstOrDefault(x => x.Id == cartItem.ProductId),
                    Quantity = cartItem.Quantity
                });
            }

            return Results.Ok(cartDetail);
        }
    }
}