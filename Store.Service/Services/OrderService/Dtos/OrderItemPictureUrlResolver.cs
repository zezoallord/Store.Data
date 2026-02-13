using AutoMapper;
using Microsoft.Extensions.Configuration;
using Store.Data.Entities.OrderEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.OrderService.Dtos
{
    public class OrderItemPictureUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly IConfiguration configuration;

        public OrderItemPictureUrlResolver(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string Resolve(OrderItem
     source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.ProductItem.PictureUrl) || !source.ProductItem.PictureUrl.Contains(source.ProductItem.PictureUrl))
            {
                return $"{configuration["BaseUrl"]}/{source.ProductItem.PictureUrl}";

            }

            return null;
        }
    }
}
