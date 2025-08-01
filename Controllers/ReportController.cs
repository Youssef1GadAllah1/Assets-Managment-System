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
            return View();
        }
            
        [HttpPost]
        public IActionResult AddReportForm(Report report)
        {
            try
            {
                if (!ModelState.IsValid)
                {
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

                _appDbContext.Reports.Add(report);
                _appDbContext.SaveChanges();

                TempData["SuccessMessage"] = "تم إضافة التقرير بنجاح";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding report: {ex.Message}");
                ModelState.AddModelError("", "حدث خطأ أثناء إضافة التقرير");
                return View("AddReport", report);
            }
        }
    }
}
