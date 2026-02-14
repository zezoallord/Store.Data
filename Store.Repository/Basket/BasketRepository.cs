using System.Text.Json;
using StackExchange.Redis;
using Store.Repository.Basket.Models;

namespace Store.Repository.Basket;

public class BasketRepository : IBasketRepository
{
    private readonly IDatabase _database;

    public BasketRepository(IConnectionMultiplexer redis)
    {
        _database = redis.GetDatabase();
    }

    public async Task<bool> DeleteBasketAsync(string basketId)
        => await _database.KeyDeleteAsync(basketId);

    public async Task<CustomerBasket> GetBasketAsync(string basketId)
    {
        var basket = await _database.StringGetAsync(basketId);
        if (basket.IsNullOrEmpty) return null;

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        return JsonSerializer.Deserialize<CustomerBasket>(basket, options);
    }

    public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
    {
        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var json = JsonSerializer.Serialize(basket, options);
        
        var isCreated = await _database.StringSetAsync(basket.Id, json, TimeSpan.FromDays(30));
        
        if (!isCreated)
            return null;
            
        return await GetBasketAsync(basket.Id);
    }
}