using System;

namespace ShopApi.DTOs
{
    public class ShoppingCartDto
    {
        public int ShoppingCartId { get; set; }

        // Time value in milliseconds
        public string DateCreated { get; set; }
    }
}
