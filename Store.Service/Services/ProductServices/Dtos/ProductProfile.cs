using AutoMapper;
using Store.Data.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.ProductServices.Dtos
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDetailsDto>()
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.Name))
                .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.Type.Name))
                .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom<ProductPictureUrlResolver>());

            CreateMap<ProductBrand, BrandTypeDetailsDto>();
            CreateMap<ProductType, BrandTypeDetailsDto>();

        }
    }
}
