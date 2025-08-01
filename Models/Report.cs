using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone_Next_Step.Models
{
    public class Report
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "عنوان التقرير مطلوب")]
        public string Title { get; set; }
        [Required(ErrorMessage = "وصف التقرير مطلوب")]
        public string Description { get; set; }
        [Required(ErrorMessage = "اسم المؤلف مطلوب")]
        public string Author { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "محتوى التقرير مطلوب")]
        public string ReportContant { get; set; }
        [Required(ErrorMessage = "يرجى اختيار مستخدم")]
        [ForeignKey("User")]
        public int CreatedById { get; set; }
       

        // Foreign key for the associated asset
        [Required(ErrorMessage = "يرجى اختيار أصل")]
        [ForeignKey("Asset")]
        public int AssetId { get; set; }
        public Asset Asset { get; set; }
    }
}
