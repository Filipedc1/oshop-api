using System;
using System.Collections.Generic;

namespace ShopApi.Data.Models
{
    public class ShoppingCart
    {
        public int ShoppingCartId { get; set; }
        public DateTime DateCreatedUtc { get; set; }

        public ICollection<ShoppingCartItem> ShoppingCartItems { get; set; }
    }
}
