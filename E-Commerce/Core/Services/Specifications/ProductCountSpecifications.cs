﻿using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    internal class ProductCountSpecifications : Specifications<Product>
    {
        public ProductCountSpecifications(ProductSpecificationsParameters parameters)
           : base(product =>
           (!parameters.BrandId.HasValue || product.BrandId == parameters.BrandId.Value)
           &&
           (!parameters.TypeId.HasValue || product.TypeId == parameters.TypeId.Value))
        {
        }
    }
}