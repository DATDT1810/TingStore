using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Order.Core.Repositories;
using Order.Infrastructure.Data;

namespace Order.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDbContext _context;

        public OrderRepository(OrderDbContext context)
        {
            _context = context;
        }
        public Task<Core.Entities.Order> AddOrder(Core.Entities.Order order) => throw new NotImplementedException();
        public Task DeleteOrder(Guid orderId) => throw new NotImplementedException();
        public Task<Core.Entities.Order> GetOrderById(Guid orderId) => throw new NotImplementedException();
        public Task<IEnumerable<Core.Entities.Order>> GetOrdersByCustomerId(int customerid) => throw new NotImplementedException();
        public Task UpdateOrder(Core.Entities.Order order) => throw new NotImplementedException();
    }
}
