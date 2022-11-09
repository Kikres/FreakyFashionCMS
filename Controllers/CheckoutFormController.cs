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
            //TODO: Här

            //Hämta inloggad användare
            var user = _customMemberService.GetLoggedInMember();

            //Hämta basket
            var basket = _basketService.GetBasket();
            if (basket == null) CurrentUmbracoPage();

            //Skapa order via service
            _orderService.CreateOrder(new Order
            {
                UmbracoMemberId = user.Id,
                OrderLines = basket.BasketItems.Values.Select(o =>
                    new OrderLine
                    {
                        ProductId = o.ProductId,
                        Quantity = o.Quantity,
                        Price = UmbracoContext.Content.GetAtRoot().DescendantsOrSelf<ProductPage>().FirstOrDefault(x => x.Id == o.ProductId).ProductPrice * o.Quantity
                    })
            });

            //Radera varukorgen
            _basketService.ClearBasket();

            //Updatera TempData och skicka tillbaka användare
            TempData["FormSuccess"] = true;
            return RedirectToCurrentUmbracoPage();
        }

        //TODO: Kolla i register partial hur vi plockar ut errors och renderar
        AddErrors(result);
        return CurrentUmbracoPage();
    }

    public IActionResult CheckoutExistingAccount()
    {
        if (!ModelState.IsValid) return CurrentUmbracoPage();

        //Hämta inloggad användare
        var user = _customMemberService.GetLoggedInMember();

        //Hämta basket
        var basket = _basketService.GetBasket();
        if (basket == null) CurrentUmbracoPage();

        //Skapa order via service
        _orderService.CreateOrder(new Order
        {
            UmbracoMemberId = user.Id,
            OrderLines = basket.BasketItems.Values.Select(o =>
                new OrderLine
                {
                    ProductId = o.ProductId,
                    Quantity = o.Quantity,
                    Price = UmbracoContext.Content.GetAtRoot().DescendantsOrSelf<ProductPage>().FirstOrDefault(x => x.Id == o.ProductId).ProductPrice * o.Quantity
                })
        });

        //Radera varukorgen
        _basketService.ClearBasket();

        //Updatera TempData och skicka tillbaka användare
        TempData["FormSuccess"] = true;
        return RedirectToCurrentUmbracoPage();
    }

    private void AddErrors(IdentityResult result)
    {
        foreach (IdentityError? error in result.Errors)
        {
            ModelState.AddModelError("registerViewModel", error.Description);
        }
    }
}