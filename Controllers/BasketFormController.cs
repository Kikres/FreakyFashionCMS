using FreakyFashion.Services;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Common.PublishedModels;
using Umbraco.Cms.Web.Website.Controllers;

namespace FreakyFashion.Controllers;

public class BasketFormController : SurfaceController
{
    private readonly IBasketService _basketService;

    public BasketFormController(
        IUmbracoContextAccessor umbracoContextAccessor,
        IUmbracoDatabaseFactory databaseFactory,
        ServiceContext services,
        AppCaches appCaches,
        IProfilingLogger profilingLogger,
        IPublishedUrlProvider publishedUrlProvider,
        IBasketService basketService) : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
    {
        _basketService = basketService;
    }

    [HttpPost]
    public IActionResult AddToCart(int id)
    {
        if (UmbracoContext?.Content?.GetAtRoot().DescendantsOrSelf<ProductPage>().Where(x => x.Id == id).First() == null)
        {
            TempData["error"] = "error";
            return RedirectToCurrentUmbracoPage();
        }

        _basketService.AddProductToBasket(id);

        TempData["success"] = "success";
        return RedirectToCurrentUmbracoPage();
    }
}