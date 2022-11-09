using FreakyFashion.Services;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Web;

namespace FreakyFashion.Components;

public class OrderTable : ViewComponent
{
    private readonly IOrderService _orderService;
    private readonly ICustomMemberService _customMemberService;
    private readonly IUmbracoContextAccessor _umbracoContextAccessor;

    public OrderTable(IOrderService orderService, IUmbracoContextAccessor umbracoContextAccessor, ICustomMemberService customMemberService)
    {
        _orderService = orderService;
        _umbracoContextAccessor = umbracoContextAccessor;
        _customMemberService = customMemberService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var viewModel = new OrderTableViewModel();

        if (_umbracoContextAccessor.TryGetUmbracoContext(out IUmbracoContext context))
        {
            var orders = _orderService.GetOrdersByMemberId(_customMemberService.GetLoggedInMember().Id);

            foreach (var order in orders)
            {
                var basketRow = new OrderRowViewModel { OrderId = order.Id, Date = order.OrderDate.ToShortDateString(), Total = order.OrderLines.Sum(o => o.Price) };
                viewModel.OrderRowViewModels.Add(basketRow);
            }
        }

        return View("OrderTable", viewModel);
    }
}

public class OrderTableViewModel
{
    public ICollection<OrderRowViewModel> OrderRowViewModels { get; set; } = new List<OrderRowViewModel>();
}

public class OrderRowViewModel
{
    public int OrderId { get; set; }
    public string Date { get; set; }
    public double Total { get; set; }
}