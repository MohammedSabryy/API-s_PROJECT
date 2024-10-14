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


        public ProductWithBrandAndTypeSpecifications(ProductSpecificationsParameters parameters)
           : base(product=>
           (!parameters.BrandId.HasValue ||product.BrandId == parameters.BrandId.Value) 
           &&
           (!parameters.TypeId.HasValue || product.TypeId == parameters.TypeId.Value)
            &&(string.IsNullOrWhiteSpace(parameters.Search) || product.Name.ToLower().Contains(parameters.Search.ToLower().Trim())))
        {
            AddInclude(Product => Product.ProductBrand);
            AddInclude(Product => Product.ProductType);

            ApplyPagination(parameters.PageIndex,parameters.PageSize);
            if (parameters.Sort != null)
            {
                switch (parameters.Sort) 
                {
                    case ProductSortingOptions.PriceDesc:
                        SetOrderByDescending(p=>p.price);
                        break;
                    case ProductSortingOptions.PriceAsc:
                        SetOrderBy(p => p.price);
                        break;
                    case ProductSortingOptions.NameDesc:
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
