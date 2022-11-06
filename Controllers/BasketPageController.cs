using FreakyFashion.Models;
using FreakyFashion.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using NUglify.Helpers;
using System.Text.Json;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Cms.Web.Common.PublishedModels;
using static Umbraco.Cms.Core.Constants.HttpContext;

namespace FreakyFashion.Controllers
{
    public class BasketPageController : RenderController
    {
        private readonly IPublishedValueFallback _publishedValueFallback;

        public BasketPageController(ILogger<RenderController> logger,
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

            var viewModel = new BasketPageViewModel(CurrentPage, _publishedValueFallback);
            viewModel.BasketViewModel = new BasketViewModel() { BasketItemViewModels = basketItemViewModels };

            return CurrentTemplate(viewModel);
        }
    }
}