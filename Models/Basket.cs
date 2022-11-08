using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Web.Common.PublishedModels;
using Umbraco.Extensions;

namespace FreakyFashion.Models;

public class Basket
{
    public IDictionary<int, BasketItem> BasketItems { get; set; } = new Dictionary<int, BasketItem>();
}

public class BasketItem
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}