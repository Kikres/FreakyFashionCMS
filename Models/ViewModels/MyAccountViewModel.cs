using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace FreakyFashion.Models.ViewModels;

public class MyAccountViewModel : MyAccountPage
{
    public MyAccountViewModel(IPublishedContent content, IPublishedValueFallback publishedValueFallback) : base(content, publishedValueFallback)
    {
    }

    public IEnumerable<OrderRowViewModel> OrderRowViewModels { get; set; }
    public IMember Member { get; set; }
}