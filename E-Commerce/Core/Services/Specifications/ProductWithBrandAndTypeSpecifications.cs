using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    internal class ProductWithBrandAndTypeSpecifications : Specifications<Product>
    {
        public ProductWithBrandAndTypeSpecifications(int id)
            : base(Product=>Product.Id == id)
        {
            AddInclude(Product => Product.ProductBrand);
            AddInclude(Product => Product.ProductType);

        }


        public ProductWithBrandAndTypeSpecifications()
           : base(null)
        {
            AddInclude(Product => Product.ProductBrand);
            AddInclude(Product => Product.ProductType);

        }
    }
}
