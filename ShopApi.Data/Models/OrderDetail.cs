using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApi.Data.Models
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int ProductQuantity { get; set; }
        public decimal ProductPrice { get; set; }

        public Order Order { get; set; }
    }
}
