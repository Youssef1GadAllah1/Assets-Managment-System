using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone_Next_Step.Models
{
    public class Asset
    {

        public int Id { get; set; } 
        public string Name { get; set; }
        public string Category { get; set; }
        public DateTime date { get; set; } = DateTime.Now;
        public string Type { get; set; }
        public int Price { get; set; }
        public string Color { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public string ImageUrl { get; set; }
        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
    }
}
