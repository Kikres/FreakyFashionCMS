using FreakyFashion.Models.ViewModels;
using FreakyFashion.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace FreakyFashion.Controllers;

public class BasketPageController : RenderController
{
    private readonly IBasketService _basketService;
    private readonly IPublishedValueFallback _publishedValueFallback;

    public BasketPageController(ILogger<RenderController> logger,
        ICompositeViewEngine compositeViewEngine,
        IUmbracoContextAccessor umbracoContextAccessor,
        IBasketService basketService,
        IPublishedValueFallback publishedValueFallback) : base(logger, compositeViewEngine, umbracoContextAccessor)
    {
        _basketService = basketService;
        _publishedValueFallback = publishedValueFallback;
    }

    public override IActionResult Index()
    {
        var basketItemViewModels = _basketService.GetBasket().BasketItems.Values.Select(o => new BasketItemViewModel
        {
            Product = UmbracoContext.Content.GetAtRoot().DescendantsOrSelf<ProductPage>().FirstOrDefault(x => x.Id == o.ProductId),
            Quantity = o.Quantity
        });

        var viewModel = new BasketViewModel(CurrentPage, _publishedValueFallback);
        viewModel.BasketTableViewModel = new BasketTableViewModel { BasketItemViewModels = basketItemViewModels };

        return CurrentTemplate(viewModel);
    }
}