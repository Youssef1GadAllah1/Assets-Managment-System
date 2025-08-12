using System.ComponentModel.DataAnnotations;

namespace Capstone_Next_Step.Models
{
    public class Notification
    {
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Message { get; set; }
        public string Type { get; set; } = "info"; // info, success, warning, error
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;
        public string? LinkUrl { get; set; }
    }
}

