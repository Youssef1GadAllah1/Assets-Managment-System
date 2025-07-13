using System.ComponentModel.DataAnnotations;

namespace Capstone_Next_Step.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter product name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter category")]
        public string Category { get; set; }
        [Required(ErrorMessage = "Please enter price")]
        [Range(0, int.MaxValue, ErrorMessage = "Price must be zero or greater")]
        public int Price { get; set; }
        [Required(ErrorMessage = "Please enter product type")]
        public string Type { get; set; }
        [Required(ErrorMessage = "Please enter count")]
        [Range(0, int.MaxValue, ErrorMessage = "Count must be zero or greater")]
        public int Count { get; set; }
        [Required(ErrorMessage = "Please enter a color")]
        public string Color { get; set; }
        // Store the image binary data
        public String ImageUrl { get; set; }
        // Store the image file name
        
    }
}
