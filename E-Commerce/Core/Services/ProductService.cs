﻿global using Services.Abstractions;
global using Shared;
global using AutoMapper;
global using Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Services
{
    internal class ProductService(IUnitOfWork UnitOfWork , IMapper Mapper) : IProductService
    {
        public async Task<IEnumerable<BrandResultDTO>> GetAllBrandsAsync()
        {
            var brands = await UnitOfWork.GetRepository<ProductBrand, int>().GetAllAsync();
            var brandsResult = Mapper.Map<IEnumerable<BrandResultDTO>>(brands);
            return brandsResult;
        }

        public async Task<IEnumerable<ProductResultDTO>> GetAllProductsAsync()
        {
            var Products = await UnitOfWork.GetRepository<Product, int>().GetAllAsync();
            var ProductResult = Mapper.Map<IEnumerable<ProductResultDTO>>(Products);
            return ProductResult;
        }

        public async Task<IEnumerable<TypeResultDTO>> GetAllTypesAsync()
        {
            var Types = await UnitOfWork.GetRepository<ProductType, int>().GetAllAsync();
            var TypesResult = Mapper.Map<IEnumerable<TypeResultDTO>>(Types);
            return TypesResult;
        }

        public async Task<ProductResultDTO?> GetProductByIdAsync(int id)
        {
            var Product = await UnitOfWork.GetRepository<Product, int>().GetAsync(id);
            var ProductResult = Mapper.Map<ProductResultDTO>(Product);
            return ProductResult;
        }
    }
}
