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

                // Ensure navbar shows latest profile image
                if (!string.IsNullOrEmpty(currentUser.ProfileImage))
                {
                    HttpContext.Session.SetString("ProfileImage", currentUser.ProfileImage);
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
                    TempData["ErrorMessage"] = "Please select an image.";
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
                    TempData["ErrorMessage"] = "Please select an image in JPG, PNG, or GIF format.";
                    return RedirectToAction("Details", new { id = id });
                }

                // Validate file size (max 5MB)
                if (profileImage.Length > 5 * 1024 * 1024)
                {
                    TempData["ErrorMessage"] = "Image size must be less than 5 MB.";
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

                // Update session so navbar can show the new photo
                HttpContext.Session.SetString("ProfileImage", user.ProfileImage ?? string.Empty);

                var isArabic = HttpContext.Session.GetString("lang") == "ar";
                TempData["SuccessMessage"] = isArabic ? "Profile photo updated successfully" : "Profile photo updated successfully";
                // redirect back to profile page to refresh avatar
                return RedirectToAction("Index", "Profile");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uploading profile image: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while uploading the image.";
                return RedirectToAction("Details", new { id = id });
            }
        }

        [HttpPost]
        public IActionResult UpdateProfile(User user)
        {
            try
            {
                // These fields are not part of the posted form; prevent automatic required validation
                ModelState.Remove("UserName");
                ModelState.Remove("Password");
                ModelState.Remove("Role");
                ModelState.Remove("JobTitle");

                if (!ModelState.IsValid)
                {
                    // Get the current user data to populate the form
                    var currentUser = _context.Users.FirstOrDefault(u => u.Id == user.Id);
                    if (currentUser != null)
                    {
                        user.UserName = currentUser.UserName;
                        user.Password = currentUser.Password;
                        user.Role = currentUser.Role;
                        user.ProfileImage = currentUser.ProfileImage;
                    }
                    return View("Index", user);
                }

                var existingUser = _context.Users.FirstOrDefault(u => u.Id == user.Id);
                if (existingUser == null)
                {
                    return NotFound();
                }

                // Update user properties (preserve existing values when fields are left empty)
                if (!string.IsNullOrWhiteSpace(user.Name))
                {
                    existingUser.Name = user.Name;
                }
                if (!string.IsNullOrWhiteSpace(user.Email))
                {
                    existingUser.Email = user.Email;
                }
                // PhoneNamber is non-nullable int; only overwrite if a value was actually posted
                if (Request.HasFormContentType && Request.Form.ContainsKey("PhoneNamber") && int.TryParse(Request.Form["PhoneNamber"], out var phone))
                {
                    existingUser.PhoneNamber = phone;
                }
                if (!string.IsNullOrWhiteSpace(user.JobTitle))
                {
                    existingUser.JobTitle = user.JobTitle;
                }

                _context.SaveChanges();

                var isArabic = HttpContext.Session.GetString("lang") == "ar";
                TempData["SuccessMessage"] = isArabic ? "Profile updated successfully" : "Profile updated successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating profile: {ex.Message}");
                ModelState.AddModelError("", "An error occurred while updating the data.");

                // Get the current user data to populate the form
                var currentUser = _context.Users.FirstOrDefault(u => u.Id == user.Id);
                if (currentUser != null)
                {
                    user.UserName = currentUser.UserName;
                    user.Password = currentUser.Password;
                    user.Role = currentUser.Role;
                    user.ProfileImage = currentUser.ProfileImage;
                }
                return View("Index", user);
            }
        }
    }
}
