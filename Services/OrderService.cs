using FreakyFashion.Models;
using Umbraco.Cms.Infrastructure.Scoping;
using static FreakyFashion.Migrations.AddOrderLineTable;
using static FreakyFashion.Migrations.AddOrderTable;

namespace FreakyFashion.Services;

public interface IOrderService
{
    void CreateOrder(Order order);

    Order GetOrder(int orderId);

    IEnumerable<Order> GetOrdersByMemberId(int userId);
}

public class OrderService : IOrderService
{
    private readonly IScopeProvider _scopeProvider;

    public OrderService(IScopeProvider scopeProvider, IBasketService basketService)
    {
        _scopeProvider = scopeProvider;
    }

    public void CreateOrder(Order order)
    {
        using var scope = _scopeProvider.CreateScope(autoComplete: true);

        var orderSchema = new OrderSchema { UmbracoMemberId = order.UmbracoMemberId, OrderDate = DateTime.Now };
        scope.Database.Insert(orderSchema);

        var orderLineSchemas = order.OrderLines.Select(o => new OrderLineSchema
        {
            OrderId = orderSchema.Id,
            ProductId = o.ProductId,
            Quantity = o.Quantity,
            Price = o.Price
        });
        scope.Database.InsertBatch(orderLineSchemas);

        scope.Complete();
    }

    public Order GetOrder(int orderId)
    {
        using var scope = _scopeProvider.CreateScope(autoComplete: true);

        var orderSchema = scope.Database.Query<OrderSchema>(@$"
                SELECT *
                FROM MemberOrder
                WHERE Id = {orderId}
            ").FirstOrDefault();

        var orderLineSchemas = scope.Database.Query<OrderLineSchema>(@$"
                SELECT *
                FROM MemberOrderLine
                WHERE OrderId = {orderSchema.Id}
            ");

        var order = new Order()
        {
            Id = orderSchema.Id,
            UmbracoMemberId = orderSchema.UmbracoMemberId,
            OrderDate = orderSchema.OrderDate,
            OrderLines = orderLineSchemas.Select(o => new OrderLine { Id = o.Id, ProductId = o.ProductId, Quantity = o.Quantity, Price = o.Price }),
        };

        scope.Complete();
        return order;
    }

    public IEnumerable<Order> GetOrdersByMemberId(int memberId)
    {
        using var scope = _scopeProvider.CreateScope(autoComplete: true);

        var orderSchemas = scope.Database.Query<OrderSchema>(@$"
                SELECT *
                FROM MemberOrder
                WHERE UmbracoMemberId = {memberId}
            ");

        var orders = new List<Order>();

        foreach (var orderSchema in orderSchemas)
        {
            var orderLineSchemas = scope.Database.Query<OrderLineSchema>(@$"
                SELECT *
                FROM MemberOrderLine
                WHERE OrderId = {orderSchema.Id}
            ");

            var order = new Order()
            {
                Id = orderSchema.Id,
                UmbracoMemberId = orderSchema.UmbracoMemberId,
                OrderDate = orderSchema.OrderDate,
                OrderLines = orderLineSchemas.Select(o => new OrderLine { Id = o.Id, ProductId = o.ProductId, Quantity = o.Quantity, Price = o.Price }),
            };

            orders.Add(order);
        }

        scope.Complete();
        return orders;
    }
}