namespace Giggle_Garments_MVC.Models
{
    public class OrderedProduct
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string Size { get; set; }
        public int Quantity { get; set; }
        
    }
}
