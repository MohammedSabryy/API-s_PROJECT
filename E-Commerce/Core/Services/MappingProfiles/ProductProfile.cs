using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappingProfiles
{
    internal class ProductProfile : Profile
    {
        public ProductProfile() 
        {
            CreateMap<Product, ProductResultDTO>().ForMember(d => d.BrandName
            , options => options
            .MapFrom(s => s.ProductBrand.Name)).ForMember(d => d.TypeName, options => options
            .MapFrom(s => s.ProductType.Name))
            .ForMember(d => d.PictureUrl, options => options.MapFrom<PictureUrlResolver>());


            CreateMap<ProductBrand, BrandResultDTO>();
            CreateMap<ProductType, TypeResultDTO>();
        }
    }
}
