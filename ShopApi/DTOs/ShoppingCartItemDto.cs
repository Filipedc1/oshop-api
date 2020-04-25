namespace ShopApi.DTOs
{
    public class ShoppingCartItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int ShoppingCartId { get; set; }
    }
}
