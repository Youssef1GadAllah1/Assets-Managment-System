using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone_Next_Step.Models
{
    public class Asset
    {

        public int Id { get; set; } 
        [Required(ErrorMessage = "اسم الأصل مطلوب")]
        public string Name { get; set; }
        [Required(ErrorMessage = "فئة الأصل مطلوبة")]
        public string Category { get; set; }
        public DateTime date { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "نوع الأصل مطلوب")]
        public string Type { get; set; }
        public int Price { get; set; }
        [Required(ErrorMessage = "لون الأصل مطلوب")]
        public string Color { get; set; }
        [Required(ErrorMessage = "موقع الأصل مطلوب")]
        public string Location { get; set; }
        [Required(ErrorMessage = "حالة الأصل مطلوبة")]
        public string Status { get; set; }
        public string? ImageUrl { get; set; }
        [Required(ErrorMessage = "يرجى اختيار مستخدم")]
        [ForeignKey("User")]
        public int UserId { get; set; }
    }
}
