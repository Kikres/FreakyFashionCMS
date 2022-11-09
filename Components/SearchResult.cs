using FreakyFashion.Services;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Web.Common.PublishedModels;
using Umbraco.Cms.Core.Web;

namespace FreakyFashion.Components;

public class SearchResult : ViewComponent
{
    private readonly ISearchService _searchService;
    private readonly IUmbracoContextAccessor _umbracoContextAccessor;

    public SearchResult(ISearchService searchService, IUmbracoContextAccessor umbracoContextAccessor)
    {
        _searchService = searchService;
        _umbracoContextAccessor = umbracoContextAccessor;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        SearchResultViewModel viewModel = new SearchResultViewModel();
        string queryString = HttpContext.Request.Query["query"];

        if (_umbracoContextAccessor.TryGetUmbracoContext(out IUmbracoContext context))
        {
            viewModel = new SearchResultViewModel()
            {
                SearchResults = _searchService.SearchContentProducts(queryString),
                HasSearched = !string.IsNullOrEmpty(queryString),
                QueryString = queryString
            };
        }

        return View("SearchResult", viewModel);
    }
}

public class SearchResultViewModel
{
    public IEnumerable<ProductPage> SearchResults { get; set; } = Enumerable.Empty<ProductPage>();
    public bool HasSearched { get; set; }
    public string QueryString { get; set; }
}