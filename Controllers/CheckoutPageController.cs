using FreakyFashion.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Cms.Web.Common.PublishedModels;
using Umbraco.Cms.Core.Models.PublishedContent;
using FreakyFashion.Services;

namespace FreakyFashion.Controllers
{
    public class CheckoutPageController : RenderController
    {
        private readonly IPublishedValueFallback _publishedValueFallback;
        private readonly IBasketService _basketService;
        private readonly ICustomMemberService _customMemberService;

        public CheckoutPageController(
            ILogger<RenderController> logger,
            ICompositeViewEngine compositeViewEngine,
            IUmbracoContextAccessor umbracoContextAccessor,
            IPublishedValueFallback publishedValueFallback,
            IBasketService basketService,
            ICustomMemberService customMemberService) : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            _publishedValueFallback = publishedValueFallback;
            _basketService = basketService;
            _customMemberService = customMemberService;
        }

        public override IActionResult Index()
        {
            var basketItemViewModels = _basketService.GetBasket().BasketItems.Values.Select(o => new BasketItemViewModel
            {
                Product = UmbracoContext.Content.GetAtRoot().DescendantsOrSelf<ProductPage>().FirstOrDefault(x => x.Id == o.ProductId),
                Quantity = o.Quantity
            });

            var viewModel = new CheckoutViewModel(CurrentPage, _publishedValueFallback);

            viewModel.BasketViewModel = new BasketTableViewModel { BasketItemViewModels = basketItemViewModels };
            viewModel.CheckoutNewFormViewModel = new Models.ViewModels.Forms.CheckoutNewFormViewModel();
            viewModel.CheckoutExistingFormViewModel = new Models.ViewModels.Forms.CheckoutExistingFormViewModel();
            viewModel.isLoggedIn = _customMemberService.IsLoggedIn();

            return CurrentTemplate(viewModel);
        }
    }
}