using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace FreakyFashion.Models.ViewModels;

public class BasketViewModel
{
    public IEnumerable<BasketItemViewModel> BasketItemViewModels { get; set; } = Enumerable.Empty<BasketItemViewModel>();
}

public class BasketItemViewModel
{
    public ProductPage Item { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
}