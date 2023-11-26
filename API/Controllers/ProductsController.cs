using core.Entities;
using core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController
    {
        private readonly IProductRepository _repo;
        public ProductsController(IProductRepository repo)
        {
            _repo = repo;
        }
        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts(){
           var products = await _repo.GetProductsAsync();
            return products.ToList();
        }

         [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id){
            var result  = await _repo.GetProductByAsync(id);
            return result;
        }

          [HttpGet("brands")]
        public async Task<ActionResult<List<ProductBrand>>> GetProductBrands(){
            var result  = await _repo.GetProductBrandsAsync();
            return result.ToList();
        }

          [HttpGet("types")]
        public async Task<ActionResult<List<ProductType>>> GetProductTypes(){
            var result  = await _repo.GetProductTypesAsync();
            return result.ToList();
        }
    }
}