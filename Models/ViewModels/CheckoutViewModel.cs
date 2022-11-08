using FreakyFashion.Models.ViewModels.Forms;
using System.ComponentModel;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Web.Common.PublishedModels;
using Umbraco.Cms.Web.Website.Models;

namespace FreakyFashion.Models.ViewModels;

public class CheckoutViewModel : CheckoutPage
{
    public CheckoutViewModel(IPublishedContent content, IPublishedValueFallback publishedValueFallback) : base(content, publishedValueFallback)
    {
    }

    public BasketTableViewModel BasketViewModel { get; set; }
    public CheckoutNewFormViewModel CheckoutNewFormViewModel { get; set; }
    public CheckoutExistingFormViewModel CheckoutExistingFormViewModel { get; set; }
    public bool isLoggedIn { get; set; }
}