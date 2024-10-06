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


        public ProductWithBrandAndTypeSpecifications(string? sort,int? brandId,int? typeId)
           : base(product=>(!brandId.HasValue ||product.BrandId == brandId.Value) 
           &&
           (!typeId.HasValue || product.TypeId == typeId.Value))
        {
            AddInclude(Product => Product.ProductBrand);
            AddInclude(Product => Product.ProductType);


            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort.ToLower().Trim()) 
                {
                    case "pricedesc":
                        SetOrderByDescending(p=>p.price);
                        break;
                    case "priceasc":
                        SetOrderBy(p => p.price);
                        break;
                    case "namedesc":
                        SetOrderByDescending(p => p.Name);
                        break;
                    default:
                        SetOrderBy(p => p.Name);
                        break;
                }
            }
        }
    }
}
