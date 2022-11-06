using FreakyFashion.Models;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Bcpg;
using Polly;
using System.Text.Json;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Common.PublishedModels;
using Umbraco.Cms.Web.Website.Controllers;

namespace FreakyFashion.Controllers
{
    public class BasketFormController : SurfaceController
    {
        public BasketFormController(
            IUmbracoContextAccessor umbracoContextAccessor,
            IUmbracoDatabaseFactory databaseFactory,
            ServiceContext services,
            AppCaches appCaches,
            IProfilingLogger profilingLogger,
            IPublishedUrlProvider publishedUrlProvider) : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
        }

        [HttpPost]
        public IActionResult AddToCart(int id)
        {
            var productContent = UmbracoContext?.Content?.GetAtRoot().DescendantsOrSelf<ProductPage>().Where(x => x.Id == id).First();

            if (!ModelState.IsValid || productContent == null || productContent == null)
            {
                TempData["error"] = "error";
                return RedirectToCurrentUmbracoPage();
            }

            var serializedBasket = HttpContext.Session.GetString("Basket");
            Basket? basket = serializedBasket != null
                ? JsonSerializer.Deserialize<Basket>(serializedBasket)
                : new Basket();

            basket.Add(id);

            serializedBasket = JsonSerializer.Serialize(basket);
            HttpContext.Session.SetString("Basket", serializedBasket);

            TempData["success"] = "success";
            return RedirectToCurrentUmbracoPage();
        }
    }
}