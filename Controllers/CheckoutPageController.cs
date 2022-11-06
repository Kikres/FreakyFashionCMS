using FreakyFashion.Models.ViewModels;
using FreakyFashion.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Cms.Web.Common.PublishedModels;
using Umbraco.Cms.Core.Models.PublishedContent;
using System.Text.Json;

namespace FreakyFashion.Controllers
{
    public class CheckoutPageController : RenderController
    {
        private readonly IPublishedValueFallback _publishedValueFallback;

        public CheckoutPageController(
            ILogger<RenderController> logger,
            ICompositeViewEngine compositeViewEngine,
            IUmbracoContextAccessor umbracoContextAccessor,
            IPublishedValueFallback publishedValueFallback) : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            _publishedValueFallback = publishedValueFallback;
        }

        public override IActionResult Index()
        {
            var serializedBasket = HttpContext.Session.GetString("Basket");

            Basket? basket = serializedBasket != null
                ? JsonSerializer.Deserialize<Basket>(serializedBasket)
                : new Basket();

            var viewModel = new CheckoutPageViewModel(CurrentPage, _publishedValueFallback);

            if(TempData["FormSuccess"] != null)
            {
                return CurrentTemplate(viewModel);
            }

            var basketItemViewModels = new List<BasketItemViewModel>();

            foreach (var basketItem in basket.BasketItems)
            {
                var item = UmbracoContext.Content.GetAtRoot().DescendantsOrSelf<ProductPage>().FirstOrDefault(y => y.Id == basketItem.Key);
                var quantity = basketItem.Value.Quantity;
                var price = item.ProductPrice * quantity;

                basketItemViewModels.Add(new BasketItemViewModel
                {
                    Item = item,
                    Quantity = quantity,
                    Price = price
                });
            }

            
            viewModel.BasketViewModel = new BasketViewModel { BasketItemViewModels = basketItemViewModels };
            viewModel.RegisterModel = new RegisterViewModel();

            return CurrentTemplate(viewModel);
        }
    }
}