using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Data
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext context)
        {
            if (!context.Orders.Any())
            {
                context.Orders.AddRange(GetPreconfiguredOrders());
                await context.SaveChangesAsync();
            }
        }

        private static IEnumerable<Order> GetPreconfiguredOrders()
        {
            return new List<Order>
            {
                new ()
                {
                    AuctionId = Guid.NewGuid().ToString(),
                    ProductId = Guid.NewGuid().ToString(),
                    SellerUserName = "test@test.com",
                    UnitPrice = 10,
                    TotalPrice = 1000,
                    CreatedAt = DateTime.UtcNow
                },
                new ()
                {
                    AuctionId = Guid.NewGuid().ToString(),
                    ProductId = Guid.NewGuid().ToString(),
                    SellerUserName = "test1@test.com",
                    UnitPrice = 10,
                    TotalPrice = 1000,
                    CreatedAt = DateTime.UtcNow
                },
                new ()
                {
                    AuctionId = Guid.NewGuid().ToString(),
                    ProductId = Guid.NewGuid().ToString(),
                    SellerUserName = "test2@test.com",
                    UnitPrice = 10,
                    TotalPrice = 1000,
                    CreatedAt = DateTime.UtcNow
                }
            };
        }
    }
}
