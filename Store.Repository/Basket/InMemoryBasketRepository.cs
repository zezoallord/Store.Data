using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using Store.Repository.Basket.Models;

namespace Store.Repository.Basket
{
    public class InMemoryBasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _cache;

        public InMemoryBasketRepository(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            await _cache.RemoveAsync(basketId);
            return true;
        }

        public async Task<CustomerBasket> GetBasketAsync(string basketId)
        {
            var data = await _cache.GetStringAsync(basketId);
            return string.IsNullOrEmpty(data) ? null : JsonSerializer.Deserialize<CustomerBasket>(data);
        }

        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
        {
            var data = JsonSerializer.Serialize(basket);
            await _cache.SetStringAsync(basket.Id, data);
            return await GetBasketAsync(basket.Id);
        }
    }
}
