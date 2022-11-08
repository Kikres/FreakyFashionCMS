using FreakyFashion.Models;
using Org.BouncyCastle.Bcpg;
using System.Text.Json;

namespace FreakyFashion.Services;

public interface IBasketService
{
    Basket GetBasket();

    void UpdateBasket(Basket basket);

    public void AddProductToBasket(int id);

    public void ClearBasket();
}

public class BasketService : IBasketService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public BasketService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Basket GetBasket()
    {
        var serializedBasket = _httpContextAccessor?.HttpContext?.Session.GetString("Basket");
        return serializedBasket != null ? JsonSerializer.Deserialize<Basket>(serializedBasket) : new Basket();
    }

    public void UpdateBasket(Basket basket)
    {
        _httpContextAccessor?.HttpContext?.Session.SetString("Basket", JsonSerializer.Serialize(basket));
    }

    public void AddProductToBasket(int id)
    {
        var basket = GetBasket();

        if (basket.BasketItems.ContainsKey(id))
        {
            basket.BasketItems[id].Quantity += 1;
        }
        else
        {
            basket.BasketItems.Add(id, new BasketItem { ProductId = id, Quantity = 1 });
        }

        UpdateBasket(basket);
    }

    public void ClearBasket()
    {
        _httpContextAccessor?.HttpContext?.Session.Clear();
    }
}