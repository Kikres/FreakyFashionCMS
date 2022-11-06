using System.ComponentModel;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace FreakyFashion.Models.ViewModels;

public class CheckoutPageViewModel : CheckoutPage
{
    public CheckoutPageViewModel(IPublishedContent content, IPublishedValueFallback publishedValueFallback) : base(content, publishedValueFallback)
    {
    }

    public BasketViewModel? BasketViewModel { get; set; }
    public RegisterViewModel? RegisterModel { get; set; }
}