using Microsoft.EntityFrameworkCore;
using ShopApi.Data;
using ShopApi.Core.Interfaces;
using ShopApi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApi.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _database;

        public OrderService(AppDbContext context)
        {
            _database = context;
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync()
        {
            return await _database.Orders
                                  .Include(x => x.OrderDetails)
                                  .Include(x => x.ShippingDetail)
                                  .Include(x => x.User)
                                  .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId)
        {
            return await _database.Orders
                                  .Include(x => x.OrderDetails)
                                  .Include(x => x.ShippingDetail)
                                  .Include(x => x.User)
                                  .Where(x => x.User.Id == userId)
                                  .ToListAsync();
        }

        public async Task AddOrderAsync(Order order, IEnumerable<OrderDetail> orderDetails)
        {
            await _database.OrderDetails.AddRangeAsync(orderDetails);
            await _database.Orders.AddAsync(order);
            await _database.SaveChangesAsync();
        }
    }
}
