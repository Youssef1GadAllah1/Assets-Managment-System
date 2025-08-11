using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone_Next_Step.Models
{
    public class Asset
    {

        public int Id { get; set; } 
        [Required(ErrorMessage = "Asset name required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Asset category required")]
        public string Category { get; set; }
        public DateTime date { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "Asset type required")]
        public string Type { get; set; }
        public int Price { get; set; }
        [Required(ErrorMessage = "Asset color required")]
        public string Color { get; set; }
        [Required(ErrorMessage = "Asset site required")]
        public string Location { get; set; }
        [Required(ErrorMessage = "Asset status required")]
        public string Status { get; set; }
        public string? ImageUrl { get; set; }
        [Required(ErrorMessage = "Please select a user")]
        [ForeignKey("User")]
        public int UserId { get; set; }
    }
}
