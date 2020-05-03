using ShopApi.Data;
using ShopApi.Data.Interfaces;
using ShopApi.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShopApi.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _database;

        public OrderService(AppDbContext context)
        {
            _database = context;
        }

        public async Task AddOrderAsync(Order order, IEnumerable<OrderDetail> orderDetails)
        {
            await _database.OrderDetails.AddRangeAsync(orderDetails);
            await _database.Orders.AddAsync(order);
            await _database.SaveChangesAsync();
        }
    }
}
