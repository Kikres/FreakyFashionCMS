using FreakyFashion.Models;
using FreakyFashion.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;

namespace FreakyFashion.Controllers
{
    public class SearchPageController : RenderController
    {
        private readonly IPublishedValueFallback _publishedValueFallback;
        private readonly ISearchService _searchService;

        public SearchPageController(ILogger<RenderController> logger,
            ICompositeViewEngine compositeViewEngine,
            IUmbracoContextAccessor umbracoContextAccessor,
            IPublishedValueFallback publishedValueFallback,
            ISearchService searchService) : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            _publishedValueFallback = publishedValueFallback;
            _searchService = searchService;
        }

        public override IActionResult Index()
        {
            // Get the queryString from the request
            string queryString = HttpContext.Request.Query["query"];

            var viewModel = new SearchViewModel(CurrentPage, _publishedValueFallback)
            {
                SearchResults = _searchService.SearchContentProducts(queryString),
                HasSearched = !string.IsNullOrEmpty(queryString),
                QueryString = queryString
            };

            return CurrentTemplate(viewModel);
        }
    }
}