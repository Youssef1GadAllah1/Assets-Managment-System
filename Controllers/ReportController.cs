using Capstone_Next_Step.Data;
using Capstone_Next_Step.Models;
using Microsoft.AspNetCore.Mvc;

namespace Capstone_Next_Step.Controllers
{
    public class ReportController : Controller
    {
        private AppDbContext _appDbContext;
        public ReportController( AppDbContext appDbContext)
        {
            
            _appDbContext = appDbContext;
        }
        public IActionResult Index()
        {
            var ReportList = _appDbContext.Reports.ToList();
            return View(ReportList);
        }
        public IActionResult AddReport()
        {
            return View();
        }
            
        public IActionResult AddReportForm(Report x)
        {
            _appDbContext.Reports.Add(x);
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
