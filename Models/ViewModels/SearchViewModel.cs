using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace FreakyFashion.Models.ViewModels;

public class SearchViewModel : SearchPage
{
    public SearchViewModel(IPublishedContent content, IPublishedValueFallback publishedValueFallback) : base(content, publishedValueFallback)
    {
    }

    public IEnumerable<ProductPage> SearchResults { get; set; } = Enumerable.Empty<ProductPage>();
    public bool HasSearched { get; set; }
    public string QueryString { get; set; }
}