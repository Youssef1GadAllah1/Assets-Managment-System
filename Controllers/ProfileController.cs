using Capstone_Next_Step.Data;
using Capstone_Next_Step.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Capstone_Next_Step.Controllers
{
    public class ProfileController : Controller
    {
        private readonly AppDbContext _context;
        
        public ProfileController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }
        
        public IActionResult Index()
        {
            try
            {
                var currentUserName = HttpContext.Session.GetString("UserName");
                if (string.IsNullOrEmpty(currentUserName))
                {
                    return RedirectToAction("Login", "Login");
                }

                var currentUser = _context.Users.FirstOrDefault(u => u.UserName == currentUserName);
                if (currentUser == null)
                {
                    return RedirectToAction("Login", "Login");
                }

                return View(currentUser);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Profile/Index: {ex.Message}");
                return RedirectToAction("Login", "Login");
            }
        }

        public IActionResult Details(int id)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Id == id);
                if (user == null)
                {
                    return NotFound();
                }
                return View(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Profile/Details: {ex.Message}");
                return NotFound();
            }
        }

        [HttpPost]
        public IActionResult UploadProfileImage(int id, IFormFile profileImage)
        {
            try
            {
                if (profileImage == null || profileImage.Length == 0)
                {
                    TempData["ErrorMessage"] = "يرجى اختيار صورة";
                    return RedirectToAction("Details", new { id = id });
                }

                var user = _context.Users.FirstOrDefault(u => u.Id == id);
                if (user == null)
                {
                    return NotFound();
                }

                // Validate file type
                var allowedTypes = new[] { "image/jpeg", "image/png", "image/gif" };
                if (!allowedTypes.Contains(profileImage.ContentType.ToLower()))
                {
                    TempData["ErrorMessage"] = "يرجى اختيار صورة بصيغة JPG أو PNG أو GIF";
                    return RedirectToAction("Details", new { id = id });
                }

                // Validate file size (max 5MB)
                if (profileImage.Length > 5 * 1024 * 1024)
                {
                    TempData["ErrorMessage"] = "حجم الصورة يجب أن يكون أقل من 5 ميجابايت";
                    return RedirectToAction("Details", new { id = id });
                }

                // Create uploads folder if it doesn't exist
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Image", "Profile");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Generate unique filename
                var uniqueFileName = $"user_{id}_{DateTime.Now:yyyyMMddHHmmss}_{Path.GetFileName(profileImage.FileName)}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Save file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    profileImage.CopyTo(stream);
                }

                // Update user profile image
                user.ProfileImage = "/Image/Profile/" + uniqueFileName;
                _context.SaveChanges();

                TempData["SuccessMessage"] = "تم تحديث صورة البروفايل بنجاح";
                return RedirectToAction("Details", new { id = id });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uploading profile image: {ex.Message}");
                TempData["ErrorMessage"] = "حدث خطأ أثناء رفع الصورة";
                return RedirectToAction("Details", new { id = id });
            }
        }

        [HttpPost]
        public IActionResult UpdateProfile(User user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Index", user);
                }

                var existingUser = _context.Users.FirstOrDefault(u => u.Id == user.Id);
                if (existingUser == null)
                {
                    return NotFound();
                }

                // Update user properties
                existingUser.Name = user.Name;
                existingUser.Email = user.Email;
                existingUser.PhoneNamber = user.PhoneNamber;
                existingUser.JobTitle = user.JobTitle;

                _context.SaveChanges();

                TempData["SuccessMessage"] = "تم تحديث البيانات بنجاح";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating profile: {ex.Message}");
                ModelState.AddModelError("", "حدث خطأ أثناء تحديث البيانات");
                return View("Index", user);
            }
        }
    }
}