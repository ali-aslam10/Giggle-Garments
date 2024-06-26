using System.ComponentModel.DataAnnotations;

namespace Giggle_Garments_MVC.Models
{
    public class Product
    {
        public int? Id { get; set; }

        [Required]
        public  string Name { get; set; }
        
        [Required]
        public string Description { get; set; }
        
        [Required]
        public decimal? Price  { get; set; }
       
        [Required]
        [Range(1,3)]
        public int? CategoryId { get; set; }
        public string?  ImagePath { get; set; }
    }
}
