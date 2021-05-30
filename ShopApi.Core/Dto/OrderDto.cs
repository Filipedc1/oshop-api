using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApi.Core.Dto
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public decimal Total { get; set; }
        public string DatePlaced { get; set; }
        public string Username { get; set; }

        public ShippingDetailDto Shipping { get; set; }
        public IEnumerable<ShoppingCartItemDto> Items { get; set; }
    }
}
