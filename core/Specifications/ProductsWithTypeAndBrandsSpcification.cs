using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using core.Entities;

namespace core.Specifications
{
    public class ProductsWithTypeAndBrandsSpcification : BaseSpecification<Product>
    {
        public ProductsWithTypeAndBrandsSpcification()
        {
            AddInclude(x=>x.ProductType);
             AddInclude(x=>x.ProductBrand);
        }

        public ProductsWithTypeAndBrandsSpcification(int id) : base(x=> x.Id == id)
        {
             AddInclude(x=>x.ProductType);
             AddInclude(x=>x.ProductBrand);
        }
    }
}