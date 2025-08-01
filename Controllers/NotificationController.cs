using Microsoft.AspNetCore.Mvc;
using Capstone_Next_Step.Data;

namespace Capstone_Next_Step.Controllers
{
    public class NotificationController : Controller
    {
        private readonly AppDbContext _context;
        
        public NotificationController(AppDbContext context)
        {
            _context = context;
        }
        
        public IActionResult Index()
        {
            try
            {
                // Get notifications for current user
                var userName = HttpContext.Session.GetString("UserName");
                var notifications = GetNotificationsForUser(userName);
                
                return View(notifications);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Notification/Index: {ex.Message}");
                return View(new List<object>());
            }
        }

        private List<object> GetNotificationsForUser(string userName)
        {
            var notifications = new List<object>();

            try
            {
                // Add sample notifications (in real app, these would come from database)
                notifications.Add(new
                {
                    Id = 1,
                    Title = "تم إضافة أصل جديد",
                    Message = "تم إضافة أصل جديد إلى النظام",
                    Type = "info",
                    Time = DateTime.Now.AddMinutes(-5).ToString("HH:mm"),
                    IsRead = false
                });

                notifications.Add(new
                {
                    Id = 2,
                    Title = "تقرير جاهز",
                    Message = "تم إنشاء التقرير الشهري بنجاح",
                    Type = "success",
                    Time = DateTime.Now.AddMinutes(-15).ToString("HH:mm"),
                    IsRead = true
                });

                notifications.Add(new
                {
                    Id = 3,
                    Title = "تنبيه مهم",
                    Message = "يوجد أصول تحتاج صيانة",
                    Type = "warning",
                    Time = DateTime.Now.AddMinutes(-30).ToString("HH:mm"),
                    IsRead = false
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting notifications: {ex.Message}");
            }

            return notifications;
        }

        [HttpPost]
        public JsonResult MarkAsRead(int notificationId)
        {
            try
            {
                // In real app, update notification status in database
                return Json(new { success = true, message = "تم تحديث حالة الإشعار" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error marking notification as read: {ex.Message}");
                return Json(new { success = false, message = "حدث خطأ في تحديث الإشعار" });
            }
        }

        [HttpPost]
        public JsonResult DeleteNotification(int notificationId)
        {
            try
            {
                // In real app, delete notification from database
                return Json(new { success = true, message = "تم حذف الإشعار" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting notification: {ex.Message}");
                return Json(new { success = false, message = "حدث خطأ في حذف الإشعار" });
            }
        }
    }
}
