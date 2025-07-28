using DAL.Models;
using Microsoft.EntityFrameworkCore;

public class OrderService
{
    public void PlaceOrder(int customerId, List<int> productIds)
    {
        using var context = new FuminiTikiSystemContext();

        var products = context.Products.Where(p => productIds.Contains(p.ProductId)).ToList();
        var total = products.Sum(p => p.Price);

        var newOrder = new Order
        {
            CustomerId = customerId,
            OrderAmount = total,
            OrderDate = DateTime.Now
        };

        context.Orders.Add(newOrder);
        context.SaveChanges();

        foreach (var product in products)
        {
            product.OrderId = newOrder.OrderId;
            context.Products.Update(product);
        }

        context.SaveChanges();
    }

    public List<Order> GetOrdersByCustomer(int customerId)
    {
        using var context = new FuminiTikiSystemContext();
        return context.Orders
            .Include(o => o.Products)
            .Where(o => o.CustomerId == customerId)
            .ToList();
    }
}
