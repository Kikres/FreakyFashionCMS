using Examine;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Web.Common;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace FreakyFashion.Services;

public interface ISearchService
{
    IEnumerable<ProductPage> SearchContentProducts(string query);
}

public class SearchServices : ISearchService
{
    private readonly IExamineManager _examineManager;
    private readonly UmbracoHelper _umbracoHelper;

    public SearchServices(IExamineManager examineManager, UmbracoHelper umbracoHelper)
    {
        _examineManager = examineManager;
        _umbracoHelper = umbracoHelper;
    }

    public IEnumerable<ProductPage> SearchContentProducts(string searchQuery)
    {
        IEnumerable<string> ids = Array.Empty<string>();

        if (!string.IsNullOrEmpty(searchQuery) && _examineManager.TryGetIndex("ExternalIndex", out IIndex? index))
        {
            var query = index
                .Searcher
                .CreateQuery("content")
                .NodeTypeAlias(ProductPage.ModelTypeAlias)
                .And()
                .Field("nodeName", searchQuery);
            ids = query.Execute().OrderByDescending(o => o.Score).Select(o => o.Id);
        }

        foreach (var id in ids)
        {
            yield return _umbracoHelper.Content(id) as ProductPage;
        }
    }
}