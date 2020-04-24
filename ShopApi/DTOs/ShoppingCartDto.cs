﻿using ShopApi.Data.Models;
using System.Collections.Generic;

namespace ShopApi.DTOs
{
    public class ShoppingCartDto
    {
        public int ShoppingCartId { get; set; }
        public string DateCreatedUtc { get; set; }

        public IEnumerable<ShoppingCartItemDto> ShoppingCartItems { get; set; }
    }
}
