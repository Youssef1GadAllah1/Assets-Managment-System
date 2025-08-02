using Microsoft.AspNetCore.Mvc;
using Capstone_Next_Step.Data;
using Capstone_Next_Step.Models;

namespace Capstone_Next_Step.Controllers
{
    public class NotificationController : Controller
    {
        private readonly AppDbContext _context;
        
        public NotificationController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("api/notifications/unread-count")]
        public IActionResult UnreadCount()
        {
            var userName = HttpContext.Session.GetString("UserName");
            if (string.IsNullOrEmpty(userName)) return Json(new { count = 0 });
            var count = _context.Notifications.Count(n => n.UserName == userName && !n.IsRead);
            return Json(new { count });
        }
        
        public IActionResult Index()
        {
            try
            {
                var userName = HttpContext.Session.GetString("UserName");
                if (string.IsNullOrEmpty(userName))
                {
                    return RedirectToAction("Login", "Login");
                }

                var notifications = _context.Notifications
                    .Where(n => n.UserName == userName)
                    .OrderByDescending(n => n.CreatedAt)
                    .ToList();

                // Mark all as read once user opens notifications page
                foreach (var n in notifications.Where(n => !n.IsRead))
                {
                    n.IsRead = true;
                }
                _context.SaveChanges();

                return View(notifications);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Notification/Index: {ex.Message}");
                return View(new List<Notification>());
            }
        }

        [HttpPost]
        public JsonResult MarkAsRead(int notificationId)
        {
            try
            {
                var notif = _context.Notifications.FirstOrDefault(n => n.Id == notificationId);
                if (notif == null)
                {
                    return Json(new { success = false });
                }
                notif.IsRead = true;
                _context.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error marking notification as read: {ex.Message}");
                return Json(new { success = false });
            }
        }

        [HttpPost]
        public JsonResult DeleteNotification(int notificationId)
        {
            try
            {
                var notif = _context.Notifications.FirstOrDefault(n => n.Id == notificationId);
                if (notif == null)
                {
                    return Json(new { success = false });
                }
                _context.Notifications.Remove(notif);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting notification: {ex.Message}");
                return Json(new { success = false });
            }
        }

        // Helper to create notifications from other actions
        [NonAction]
        public void CreateNotification(string userName, string title, string message, string type = "info", string? linkUrl = null)
        {
            var notif = new Notification
            {
                UserName = userName,
                Title = title,
                Message = message,
                Type = type,
                LinkUrl = linkUrl
            };
            _context.Notifications.Add(notif);
            _context.SaveChanges();
        }
    }
}
