using System.Diagnostics;
using Capstone_Next_Step.Data;
using Capstone_Next_Step.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Capstone_Next_Step.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private AppDbContext _appDbContext;
        public HomeController(ILogger<HomeController> logger, AppDbContext appDbContext)
        {
            _logger = logger;
            _appDbContext = appDbContext;
        }

        public IActionResult Index()
        {
            var totalUsers = _appDbContext.Users.Count();
            var totalAssets = _appDbContext.Products.Count();
            var totalPendingTasks = _appDbContext.DTasks.Where(t => t.Status == "Pending").Count();

            var tasks = _appDbContext.DTasks.GroupBy(t => t.Status)
                .Select(g => new
                {
                    Status = g.Key,
                    Count = g.Count()
                })
                .ToList();

            ViewBag.TotalUsers = totalUsers;
            ViewBag.TotalAssets = totalAssets;
            ViewBag.TotalPendingTasks = totalPendingTasks;
            ViewBag.Tasks = tasks;

            return View();
        }
        
        public IActionResult Employee()
        {
            var EmployeeList = _appDbContext.Users.ToList();
                return View(EmployeeList);
        }
        public IActionResult AddEmployee()
        {
            return View();
        }
        public IActionResult AddEmployeeForm(User x)
        {
            _appDbContext.Users.Add(x);
            _appDbContext.SaveChanges();
            return RedirectToAction("Employee");
        }



        public IActionResult Map()
        {
            return View();
        }

    }

}
