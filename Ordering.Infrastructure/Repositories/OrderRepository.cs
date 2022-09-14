using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Entities;
using Ordering.Domain.Repositories;
using Ordering.Infrastructure.Repositories.Base;

namespace Ordering.Infrastructure.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(OrderContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Order>> GetOrdersBySellerUserName(string sellerUserName)
        {
            return await _dbContext
                .Orders
                .Where(o => o.SellerUserName == sellerUserName)
                .ToListAsync();
        }
    }
}
