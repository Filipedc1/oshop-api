using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApi.Data.Models
{
    public class ShoppingCartItem
    {
        public int ShoppingCartItemId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public ShoppingCart ShoppingCart { get; set; }
    }
}
