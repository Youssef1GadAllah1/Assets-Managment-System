using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone_Next_Step.Models
{
    public class Report
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Report title is required")]
        [Required(ErrorMessage = "Report title is required")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Report description required")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Author name required")]
        public string Author { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "Report content is required")]
        public string ReportContant { get; set; }
        [Required(ErrorMessage = "Please select a user")]
        [ForeignKey("User")]
        public int CreatedById { get; set; }
       

        // Foreign key for the associated asset
        [Required(ErrorMessage = "Please select an origin")]
        [ForeignKey("Asset")]
        public int AssetId { get; set; }
        public Asset Asset { get; set; }
    }
}
