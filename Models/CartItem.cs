namespace Giggle_Garments_MVC.Models
{
    public class CartItem
    {
        public string Id { get; set; }
        public string CartId { get; set; }
        public int ProdId { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
    }
}
