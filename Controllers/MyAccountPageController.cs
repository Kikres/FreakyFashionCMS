using FreakyFashion.Models;
using FreakyFashion.Models.ViewModels;
using FreakyFashion.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace FreakyFashion.Controllers
{
    public class MyAccountPageController : RenderController
    {
        private readonly IPublishedValueFallback _publishedValueFallback;
        private readonly IOrderService _orderService;
        private readonly ICustomMemberService _customMemberService;

        public MyAccountPageController(ILogger<RenderController> logger,
            ICompositeViewEngine compositeViewEngine,
            IUmbracoContextAccessor umbracoContextAccessor,
            IPublishedValueFallback publishedValueFallback,
            IOrderService orderService,
            ICustomMemberService customMemberService) : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            _publishedValueFallback = publishedValueFallback;
            _orderService = orderService;
            _customMemberService = customMemberService;
        }

        public override IActionResult Index()
        {
            var viewModel = new MyAccountViewModel(CurrentPage, _publishedValueFallback);
            var loggedInMember = _customMemberService.GetLoggedInMember();

            var orders = _orderService.GetOrdersByMemberId(loggedInMember.Id);

            viewModel.OrderRowViewModels = orders.Select(o => new OrderRowViewModel { OrderId = o.Id, Date = o.OrderDate.ToShortDateString(), Total = o.OrderLines.Sum(o => o.Price) });
            viewModel.Member = loggedInMember;

            return CurrentTemplate(viewModel);
        }
    }
}