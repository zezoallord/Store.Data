using AutoMapper;
using Store.Repository.Basket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.BasketService.Dtos
{
    public class BasketProfile : Profile
    {
        public BasketProfile()
        {
            CreateMap<CustomerBasket, CustomerBasketDto>().ReverseMap();

            CreateMap<BasketItem, BasketItemDto>()
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.ProductPrice));

            CreateMap<BasketItemDto, BasketItem>()
                .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.Price));
        }
    }
}
