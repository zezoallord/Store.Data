using Store.Data.Entities;
using Store.Service.Services.OrderService.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.OrderService
{
    public interface IOrderService
    {
        public Task<OrderDetailsDto> CreateOrderAsync(OrderDto input);

        public Task<IReadOnlyList<OrderDetailsDto>> GetAllOrdersForUserAsync(string buyerEmail);



        public Task<OrderDetailsDto> GetOrderByIdAsync(Guid id);


        public Task<IReadOnlyList<DeliveryMethod>> GetAllDeliveryMethodsAsync();
       
    
}
}
