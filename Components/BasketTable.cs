using FreakyFashion.Services;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Web.Common.PublishedModels;
using Umbraco.Cms.Core.Web;

namespace FreakyFashion.Components;

public class BasketTable : ViewComponent
{
    private readonly IBasketService _basketService;
    private readonly IUmbracoContextAccessor _umbracoContextAccessor;

    public BasketTable(IBasketService basketService, IUmbracoContextAccessor umbracoContextAccessor)
    {
        _basketService = basketService;
        _umbracoContextAccessor = umbracoContextAccessor;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var viewModel = new BasketTableViewModel();

        if (_umbracoContextAccessor.TryGetUmbracoContext(out IUmbracoContext context))
        {
            var basketItems = _basketService.GetBasket().BasketItems.Values;
            foreach (var item in basketItems)
            {
                var productPage = context.Content.GetAtRoot().DescendantsOrSelf<ProductPage>().FirstOrDefault(x => x.Id == item.ProductId);
                var basketRow = new BasketRowViewModel { Name = productPage.ProductTitle, Quantity = item.Quantity, Price = productPage.ProductPrice * item.Quantity };
                viewModel.BasketRows.Add(basketRow);
            }
        }

        return View("BasketTable", viewModel);
    }
}

public class BasketTableViewModel
{
    public ICollection<BasketRowViewModel> BasketRows { get; set; } = new List<BasketRowViewModel>();
}

public class BasketRowViewModel
{
    public string Name { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
}