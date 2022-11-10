using FreakyFashion.Models;
using Org.BouncyCastle.Bcpg;
using System.Text.Json;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace FreakyFashion.Services;

public interface IBasketService
{
    Basket GetBasket();

    public bool AddProductToBasket(int id);

    public void ClearBasket();
}

public class BasketService : IBasketService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUmbracoContextAccessor _umbracoContextAccessor;

    public BasketService(IHttpContextAccessor httpContextAccessor, IUmbracoContextAccessor umbracoContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _umbracoContextAccessor = umbracoContextAccessor;
    }

    public Basket GetBasket()
    {
        var serializedBasket = _httpContextAccessor?.HttpContext?.Session.GetString("Basket");
        return serializedBasket != null ? JsonSerializer.Deserialize<Basket>(serializedBasket) : new Basket();
    }

    public bool AddProductToBasket(int id)
    {
        if (_umbracoContextAccessor.TryGetUmbracoContext(out IUmbracoContext context))
        {
            if (context.Content.GetAtRoot().DescendantsOrSelf<ProductPage>().FirstOrDefault(o => o.Id == id) == null) return false;

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
            return true;
        }
        return false;
    }

    public void ClearBasket()
    {
        _httpContextAccessor?.HttpContext?.Session.Clear();
    }

    private void UpdateBasket(Basket basket)
    {
        _httpContextAccessor?.HttpContext?.Session.SetString("Basket", JsonSerializer.Serialize(basket));
    }
}