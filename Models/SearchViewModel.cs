using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace FreakyFashion.Models;

public class SearchViewModel : PublishedContentWrapped
{
    public SearchViewModel(IPublishedContent content, IPublishedValueFallback publishedValueFallback)
        : base(content, publishedValueFallback)
    {
    }

    public IEnumerable<ProductPage> SearchResults { get; set; } = Enumerable.Empty<ProductPage>();
    public bool HasSearched { get; set; }
    public string QueryString { get; set; }
}