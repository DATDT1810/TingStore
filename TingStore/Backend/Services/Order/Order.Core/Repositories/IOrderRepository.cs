using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Order.Core.Entities;

namespace Order.Core.Repositories
{
    public interface IOrderRepository
    {
        Task<Order.Core.Entities.Order> AddOrder(Order.Core.Entities.Order order);
        Task<Order.Core.Entities.Order> GetOrderById(Guid orderId);
        Task<IEnumerable<Order.Core.Entities.Order>> GetOrdersByCustomerId(int customerid);
        Task UpdateOrder(Order.Core.Entities.Order order);
        Task DeleteOrder(Guid orderId);
    }
}
