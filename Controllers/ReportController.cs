using Capstone_Next_Step.Data;
using Capstone_Next_Step.Models;
using Microsoft.AspNetCore.Mvc;

namespace Capstone_Next_Step.Controllers
{
    public class ReportController : Controller
    {
        private readonly AppDbContext _appDbContext;
        
        public ReportController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        
        public IActionResult Index()
        {
            try
            {
                var reports = _appDbContext.Reports
                    .OrderByDescending(r => r.Created)
                    .ToList();
                    
                return View(reports);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Report/Index: {ex.Message}");
                return View(new List<Report>());
            }
        }
        
        public IActionResult AddReport()
        {
            try
            {
                // Check if user is logged in
                var currentUserName = HttpContext.Session.GetString("UserName");
                if (string.IsNullOrEmpty(currentUserName))
                {
                    return RedirectToAction("Login", "Login");
                }

                // Get all users and assets for the dropdowns
                var users = _appDbContext.Users.ToList();
                var assets = _appDbContext.Assets.ToList();
                
                ViewBag.Users = users;
                ViewBag.Assets = assets;
                return View(new Report()); // Pass empty model
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddReport: {ex.Message}");
                ViewBag.Users = new List<User>();
                ViewBag.Assets = new List<Asset>();
                return View(new Report()); // Pass empty model
            }
        }
            
        [HttpPost]
        public IActionResult AddReportForm(Report report)
        {
            try
            {
                // Check if user is logged in
                var currentUserName = HttpContext.Session.GetString("UserName");
                if (string.IsNullOrEmpty(currentUserName))
                {
                    return RedirectToAction("Login", "Login");
                }

                if (!ModelState.IsValid)
                {
                    // Get users and assets for the dropdowns when returning with errors
                    var users = _appDbContext.Users.ToList();
                    var assets = _appDbContext.Assets.ToList();
                    ViewBag.Users = users;
                    ViewBag.Assets = assets;
                    return View("AddReport", report);
                }

                // Set creation date if not provided
                if (report.Created == default)
                {
                    report.Created = DateTime.Now;
                }

                // Set author if not provided
                if (string.IsNullOrEmpty(report.Author))
                {
                    report.Author = HttpContext.Session.GetString("Name") ?? "Unknown";
                }

                // Validate that the user exists
                var user = _appDbContext.Users.FirstOrDefault(u => u.Id == report.CreatedById);
                if (user == null)
                {
                    ModelState.AddModelError("CreatedById", "يرجى اختيار مستخدم صحيح");
                    var users = _appDbContext.Users.ToList();
                    var assets = _appDbContext.Assets.ToList();
                    ViewBag.Users = users;
                    ViewBag.Assets = assets;
                    return View("AddReport", report);
                }

                // Validate that the asset exists
                var asset = _appDbContext.Assets.FirstOrDefault(a => a.Id == report.AssetId);
                if (asset == null)
                {
                    ModelState.AddModelError("AssetId", "يرجى اختيار أصل صحيح");
                    var users = _appDbContext.Users.ToList();
                    var assets = _appDbContext.Assets.ToList();
                    ViewBag.Users = users;
                    ViewBag.Assets = assets;
                    return View("AddReport", report);
                }

                _appDbContext.Reports.Add(report);
                _appDbContext.SaveChanges();

                var isArabic = HttpContext.Session.GetString("lang") == "ar";
                TempData["SuccessMessage"] = isArabic ? "تم إضافة التقرير بنجاح!" : "Report added successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding report: {ex.Message}");
                ModelState.AddModelError("", "حدث خطأ أثناء إضافة التقرير");
                
                // Get users and assets for the dropdowns when returning with errors
                var users = _appDbContext.Users.ToList();
                var assets = _appDbContext.Assets.ToList();
                ViewBag.Users = users;
                ViewBag.Assets = assets;
                return View("AddReport", report);
            }
        }
    }
}
