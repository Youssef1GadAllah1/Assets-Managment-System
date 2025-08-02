using System.Diagnostics;
using Capstone_Next_Step.Data;
using Capstone_Next_Step.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Capstone_Next_Step.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _appDbContext;
        
        public HomeController(ILogger<HomeController> logger, AppDbContext appDbContext)
        {
            _logger = logger;
            _appDbContext = appDbContext;
        }

        public IActionResult Index()
        {
            try
            {
                var dashboardData = new
                {
                    TotalUsers = _appDbContext.Users.Count(),
                    TotalAssets = _appDbContext.Products.Count(),
                    TotalPendingTasks = _appDbContext.DTasks.Count(t => t.Status == "Pending"),
                    Tasks = _appDbContext.DTasks
                        .GroupBy(t => t.Status)
                        .Select(g => new { Status = g.Key, Count = g.Count() })
                        .ToList()
                };

                ViewBag.TotalUsers = dashboardData.TotalUsers;
                ViewBag.TotalAssets = dashboardData.TotalAssets;
                ViewBag.TotalPendingTasks = dashboardData.TotalPendingTasks;
                ViewBag.Tasks = dashboardData.Tasks;

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Home/Index");
                
                // Set default values on error
                ViewBag.TotalUsers = 0;
                ViewBag.TotalAssets = 0;
                ViewBag.TotalPendingTasks = 0;
                ViewBag.Tasks = new List<object>();
                
                return View();
            }
        }
        
        public IActionResult Employee()
        {
            try
            {
                var employees = _appDbContext.Users
                    .Where(u => u.Role != "Admin")
                    .OrderBy(u => u.Name)
                    .ToList();
                
                return View(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Home/Employee");
                return View(new List<User>());
            }
        }

        public IActionResult AddEmployee()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddEmployee(User user)
        {
            try
            {
                // Clear ProfileImage validation error if it's empty
                if (ModelState.ContainsKey("ProfileImage") && ModelState["ProfileImage"].Errors.Count > 0)
                {
                    ModelState["ProfileImage"].Errors.Clear();
                }

                if (!ModelState.IsValid)
                {
                    return View(user);
                }

                // Check if username already exists
                var existingUser = _appDbContext.Users.FirstOrDefault(u => u.UserName == user.UserName);
                if (existingUser != null)
                {
                    ModelState.AddModelError("UserName", "Username already exists");
                    return View(user);
                }

                // Check if email already exists
                var existingEmail = _appDbContext.Users.FirstOrDefault(u => u.Email == user.Email);
                if (existingEmail != null)
                {
                    ModelState.AddModelError("Email", "Email already exists");
                    return View(user);
                }

                // Validate required fields
                if (string.IsNullOrEmpty(user.Name))
                {
                    ModelState.AddModelError("Name", "Name is required");
                    return View(user);
                }

                if (string.IsNullOrEmpty(user.UserName))
                {
                    ModelState.AddModelError("UserName", "Username is required");
                    return View(user);
                }

                if (string.IsNullOrEmpty(user.Email))
                {
                    ModelState.AddModelError("Email", "Email is required");
                    return View(user);
                }

                if (string.IsNullOrEmpty(user.Password))
                {
                    ModelState.AddModelError("Password", "Password is required");
                    return View(user);
                }

                if (string.IsNullOrEmpty(user.JobTitle))
                {
                    ModelState.AddModelError("JobTitle", "Job Title is required");
                    return View(user);
                }

                // Set default role if not specified
                if (string.IsNullOrEmpty(user.Role))
                {
                    user.Role = "User";
                }

                // Set default profile image if not specified
                if (string.IsNullOrEmpty(user.ProfileImage))
                {
                    user.ProfileImage = "/Image/default-avatar.png";
                }

                _appDbContext.Users.Add(user);
                _appDbContext.SaveChanges();

                var isArabic = HttpContext.Session.GetString("lang") == "ar";
                TempData["SuccessMessage"] = isArabic ? "تم إضافة الموظف بنجاح" : "Employee added successfully";
                return RedirectToAction("Employee");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding employee");
                var isArabic2 = HttpContext.Session.GetString("lang") == "ar";
                ModelState.AddModelError("", isArabic2 ? "حدث خطأ أثناء إضافة الموظف" : "An error occurred while adding the employee");
                return View(user);
            }
        }

        public IActionResult Map()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SetLanguage(string lang)
        {
            try
            {
                if (!string.IsNullOrEmpty(lang) && (lang == "ar" || lang == "en"))
                {
                    HttpContext.Session.SetString("lang", lang);
                    return Json(new { success = true });
                }
                return Json(new { success = false, message = "Invalid language" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting language");
                return Json(new { success = false, message = "Error setting language" });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
