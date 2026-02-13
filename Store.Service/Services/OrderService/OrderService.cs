using AutoMapper;
using Store.Data.Entites;
using Store.Data.Entities;
using Store.Data.Entities.OrderEntities;
using Store.Repository.Basket.Models;
using Store.Repository.Interfaces;
using Store.Repository.Repositories;
using Store.Repository.Specefication.OrderSpecs;
using Store.Service.Services.BasketService;
using Store.Service.Services.OrderService.Dtos;
using Store.Service.Services.PaymentService;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product = Store.Data.Entites.Product;

namespace Store.Service.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IBasketService _basketService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPaymentService _paymentService;

        public OrderService(IBasketService basketService,IUnitOfWork unitOfWork,IMapper mapper,IPaymentService paymentService)
        {
            _basketService = basketService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _paymentService = paymentService;
        }
        public async Task<OrderDetailsDto> CreateOrderAsync(OrderDto input)
        {
            #region Get Basket
            var basket = await _basketService.GetBasketAsync(input.BasketId);

            if (basket is null)
            {
                throw new Exception("Basket Not Exist");
            }
            #endregion

            #region Fill Order Item List with Items in the basket
            var orderItems = new List<OrderItemDto>();

            foreach (var basketItem in basket.BasketItems)
            {
               
                var productItem = await _unitOfWork.Repository<Product, int>().GetByIdAsync(basketItem.ProductId);

                if (productItem is null)
                {
                    throw new Exception($"Product With Id: {basketItem.ProductId} Not Exist");
                }

                var itemOrdered = new ProductItem
                {
                    ProductId = productItem.Id,
                    ProductName = productItem.Name,
                    PictureUrl = productItem.PictureUrl
                };
                 var orderItem = new OrderItem
                 {
                 Price = productItem.Price,
                 Quantity = basketItem.Quantity,
                 ProductItem = itemOrdered
                     };
               var mappedOrderItem = _mapper.Map<OrderItemDto>(orderItem);
               orderItems.Add(mappedOrderItem);
            }
            #endregion

            #region get delivery method
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod, int>().GetByIdAsync(input.DeliveryMethodId); 
            if (deliveryMethod is null)
                throw new Exception("Delivery Method Not Found");
            #endregion

            #region calculate subtotal
            var subTotal = orderItems.Sum(item => item.Price * item.Quantity);
            #endregion

            #region to do payment
            var spec = new OrderWithPaymentIntentSpecefication(basket.PaymentIntentId);
            var existingorder = await _unitOfWork.Repository<Order, Guid>().GetBySpecificatiobIdAsync(spec);
            if (existingorder is not null)
            {
                await _paymentService.CreateOrUpdatePaymentIntent(basket);
            }
            #endregion

            #region create order
            var mappedshippingAddress = _mapper.Map<ShippingAddress>(input.ShippingAddress);
            var mappedorderItems = _mapper.Map<List<OrderItem>>(orderItems);
            var order = new Order
            {
                DeliveryMethod = deliveryMethod,
                ShippingAddress = mappedshippingAddress,
                BuyerEmail = input.BuyerEmail,
                BasketId = input.BasketId,
                OrderItems = mappedorderItems,
                Subtotal = subTotal,
                PaymentIntentId = basket.PaymentIntentId
            };
            await _unitOfWork.Repository<Order, Guid>().AddAsync(order);
            await _unitOfWork.CompleteAsync();
            var mappedOrder = _mapper.Map<OrderDetailsDto>(order);
            return mappedOrder;
            #endregion
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetAllDeliveryMethodsAsync()
        => await _unitOfWork.Repository<DeliveryMethod, int>().GetAllAsync();

        public async Task<IReadOnlyList<OrderDetailsDto>> GetAllOrdersForUserAsync(string buyerEmail)
        {
            var specs = new OrderWithItemSpecefication(buyerEmail);
            var orders = await _unitOfWork.Repository<Order, Guid>().GetAllwithspecificationAsync(specs);
            if (!orders.Any())
            throw new Exception("u dont have any orders yet");
            var mappedOrders = _mapper.Map<List<OrderDetailsDto>>(orders);
            return mappedOrders;

        }

        public async Task<OrderDetailsDto> GetOrderByIdAsync(Guid id)
        {
            var specs = new OrderWithItemSpecefication(id);
            var order = await _unitOfWork.Repository<Order, Guid>().GetBySpecificatiobIdAsync(specs);
            if (order is null)
                throw new Exception($"there is no order with id {id}");
            var mappedOrder = _mapper.Map < OrderDetailsDto >(order);
            return mappedOrder;

        }
    }
}
