namespace ShopApi.Core.Dto
{
    public class ShoppingCartItemDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }
        public int CategoryId { get; set; }

        public int ShoppingCartId { get; set; }
    }
}
