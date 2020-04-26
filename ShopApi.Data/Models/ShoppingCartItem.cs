namespace ShopApi.Data.Models
{
    public class ShoppingCartItem
    {
        public int ShoppingCartItemId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }
        public int CategoryId { get; set; }

        public ShoppingCart ShoppingCart { get; set; }
    }
}
