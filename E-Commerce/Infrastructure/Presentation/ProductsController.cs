using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared;
using Shared.ErrorModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(IServiceManager ServiceManager) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<PaginatedResult<ProductResultDTO>>> GetAllProducts([FromQuery]ProductSpecificationsParameters parameters)
        {
            var Products =await ServiceManager.ProductService.GetAllProductsAsync(parameters);
            return Ok(Products);
        }

        [HttpGet("Brands")]
        public async Task<ActionResult<IEnumerable<BrandResultDTO>>> GetAllBrands()
        {
            var Brands = await ServiceManager.ProductService.GetAllBrandsAsync();
            return Ok(Brands);
        }

        [HttpGet("Types")]
        public async Task<ActionResult<IEnumerable<TypeResultDTO>>> GetAllTypes()
        {
            var Types = await ServiceManager.ProductService.GetAllTypesAsync();
            return Ok(Types);
        }

        [ProducesResponseType(typeof(ErrorDetails),(int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorDetails), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ValidationErrorResponse),(int)HttpStatusCode.BadRequest)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResultDTO>> GetProduct(int id)
        {
            var Product = await ServiceManager.ProductService.GetProductByIdAsync(id);
            return Ok(Product);
        }
    }
}
