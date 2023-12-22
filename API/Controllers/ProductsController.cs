using API.DTOS;
using AutoMapper;
using core.Entities;
using core.Interfaces;
using core.Specifications;
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
        private readonly IGenericRepository<Product> _productsRepo;
        private readonly  IGenericRepository<ProductBrand> _productBrandRepo;
        private readonly IGenericRepository<ProductType> _productTypeRepo;
        private readonly IMapper _mapper;
        public ProductsController(IGenericRepository<Product> productsRepo,
        IGenericRepository<ProductBrand> productBrandRepo,
        IGenericRepository<ProductType> productTypeRepo,
        IMapper mapper )
        {
            _productsRepo = productsRepo;
            _productBrandRepo = productBrandRepo;
            _productTypeRepo = productTypeRepo;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<PRoductReturnDTO>>> GetProducts(){
            var spec = new ProductsWithTypeAndBrandsSpcification();
           var products = await _productsRepo.ListAsync(spec);
            return _mapper.Map<IReadOnlyList<Product>,IReadOnlyList<PRoductReturnDTO>>(products).ToList();
        }

         [HttpGet("{id}")]
        public async Task<ActionResult<PRoductReturnDTO>> GetProduct(int id){
            var spec = new ProductsWithTypeAndBrandsSpcification(id);
            var result  = await _productsRepo.GetEntityWithSpec(spec);
            return _mapper.Map<Product,PRoductReturnDTO>(result);
        }

          [HttpGet("brands")]
        public async Task<ActionResult<List<ProductBrand>>> GetProductBrands(){
            var result  = await _productBrandRepo.ListAllAsync();
            return result.ToList();
        }

          [HttpGet("types")]
        public async Task<ActionResult<List<ProductType>>> GetProductTypes(){
            var result  = await _productTypeRepo.ListAllAsync();
            return result.ToList();
        }
    }
}