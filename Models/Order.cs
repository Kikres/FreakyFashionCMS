namespace FreakyFashion.Models;

public class Order
{
    public int Id { get; set; }
    public int UmbracoMemberId { get; set; }
    public DateTime OrderDate { get; set; }
    public IEnumerable<OrderLine> OrderLines { get; set; }
}

public class OrderLine
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
}