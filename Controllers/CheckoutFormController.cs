using FreakyFashion.Components;
using FreakyFashion.Models;
using FreakyFashion.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Common.Filters;
using Umbraco.Cms.Web.Common.PublishedModels;
using Umbraco.Cms.Web.Website.Controllers;

namespace FreakyFashion.Controllers;

public class CheckoutFormController : SurfaceController
{
    private readonly ICustomMemberService _customMemberService;
    private readonly IOrderService _orderService;
    private readonly IBasketService _basketService;

    public CheckoutFormController(
        IUmbracoContextAccessor umbracoContextAccessor,
        IUmbracoDatabaseFactory databaseFactory,
        ServiceContext services,
        AppCaches appCaches,
        IProfilingLogger profilingLogger,
        IPublishedUrlProvider publishedUrlProvider,
        ICustomMemberService customMemberService,
        IOrderService orderService,
        IBasketService basketService) : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
    {
        _customMemberService = customMemberService;
        _orderService = orderService;
        _basketService = basketService;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [ValidateUmbracoFormRouteString]
    public async Task<IActionResult> CheckoutNewAccount(CheckoutNewFormViewModel model)
    {
        if (!ModelState.IsValid) return CurrentUmbracoPage();

        IdentityResult result = await _customMemberService.RegisterMemberAsync(model.Email, model.FirstName, model.LastName, model.Password);

        if (result.Succeeded)
        {
            CreateOrder();

            TempData["FormSuccess"] = true;
            return RedirectToCurrentUmbracoPage();
        }

        return CurrentUmbracoPage();
    }

    public IActionResult CheckoutExistingAccount()
    {
        CreateOrder();

        TempData["FormSuccess"] = true;
        return RedirectToCurrentUmbracoPage();
    }

    private void CreateOrder()
    {
        var user = _customMemberService.GetLoggedInMember();

        var basket = _basketService.GetBasket();
        if (basket == null) CurrentUmbracoPage();

        _orderService.CreateOrder(new Order
        {
            UmbracoMemberId = user.Id,
            OrderLines = basket.BasketItems.Values.Select(o =>
                new OrderLine
                {
                    ProductId = o.ProductId,
                    Quantity = o.Quantity,
                    Price = UmbracoContext.Content
                    .GetAtRoot()
                    .DescendantsOrSelf<ProductPage>()
                    .FirstOrDefault(x => x.Id == o.ProductId).ProductPrice * o.Quantity
                })
        });

        _basketService.ClearBasket();
    }
}