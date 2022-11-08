using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace FreakyFashion.Models.ViewModels;

public class BasketViewModel : BasketPage
{
    public BasketViewModel(IPublishedContent content, IPublishedValueFallback publishedValueFallback) : base(content, publishedValueFallback)
    {
    }

    public BasketTableViewModel BasketTableViewModel { get; set; }
}