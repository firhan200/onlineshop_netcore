using Microsoft.AspNetCore.Mvc;
using Schema;
using Repositories.ProductRepository;
using DTOs.Products;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Repositories;

namespace Controllers{
    [Route("Api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase {
        private readonly IGenericRepository<Product> _productRepository;
        public ProductsController(
            IGenericRepository<Product> productRepository
        ){
            _productRepository = productRepository;
        }

        [HttpGet(Name = "Get All Products")]
        [Produces("application/json")]
        public ActionResult<List<Product>> GetAllProducts(){
            List<Product> products = _productRepository.GetAll();

            return products;
        }

        [HttpPost(Name = "Create Products")]
        [Authorize(Roles = nameof(Enums.Roles.Administrator))]
        public ActionResult<bool> CreateProducts([FromBody]AddEditProductDTO product){
            Product productObj = new Product{
                PhotoUrl = product.PhotoUrl,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CreatedAt = DateTime.UtcNow
            };
            _productRepository.Create(productObj);
            return true;
        }

        [HttpPut("{Id}", Name = "Update Product By Id")]
        [Authorize(Roles = nameof(Enums.Roles.Administrator))]
        public ActionResult<bool> UpdateProductById(int Id, [FromBody]AddEditProductDTO product){
            Product productObj = new Product(){
                Id = Id,
                PhotoUrl = product.PhotoUrl,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price
            };
            _productRepository.Update(productObj);
            return true;
        }

        [HttpDelete("{Id}", Name = "Delete Product By Id")]
        [Authorize(Roles = nameof(Enums.Roles.Administrator))]
        public ActionResult<bool> DeleteProductById(int Id){
            //get product
            Product? product = _productRepository.GetById(Id);
            if(product == null){
                return false;
            }

            //product found
            _productRepository.Delete(Id);

            return true;
        }
    }
}