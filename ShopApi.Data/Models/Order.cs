using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApi.Data.Models
{
    public class Order
    {
        public int OrderId          { get; set; }
        public decimal Total        { get; set; }
        public DateTime PlacedUtc   { get; set; }

        public ShippingDetail ShippingDetail { get; set; }
        public AppUser User { get; set; }

        public IEnumerable<OrderDetail> OrderDetails { get; set; }
    }
}
