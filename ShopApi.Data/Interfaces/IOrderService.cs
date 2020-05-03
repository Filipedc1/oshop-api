using ShopApi.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShopApi.Data.Interfaces
{
    public interface IOrderService
    {
        Task AddOrderAsync(Order order, IEnumerable<OrderDetail> orderDetails);
    }
}
