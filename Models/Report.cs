using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone_Next_Step.Models
{
    public class Report
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public string ReportContant { get; set; }
        [Required]
        [ForeignKey("User")]
        public int CreatedById { get; set; }
       

        // Foreign key for the associated asset
        [Required]
        [ForeignKey("Asset")]
        public int AssetId { get; set; }
        public Asset Asset { get; set; }
    }
}
