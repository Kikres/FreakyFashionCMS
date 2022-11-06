using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace FreakyFashion.Models;

public class Basket
{
    public IDictionary<int, BasketItem> BasketItems { get; set; } = new Dictionary<int, BasketItem>();

    public void Add(int id)
    {
        if (BasketItems.ContainsKey(id))
        {
            BasketItems[id].Quantity += 1;
        }
        else
        {
            BasketItems.Add(id, new BasketItem { Id = id, Quantity = 1 });
        }
    }
}

public class BasketItem
{
    public int Id { get; set; }
    public int Quantity { get; set; }
}