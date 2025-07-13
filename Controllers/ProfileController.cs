using Capstone_Next_Step.Data;
using Microsoft.AspNetCore.Mvc;

namespace Capstone_Next_Step.Controllers
{
    public class ProfileController : Controller
    {
        private AppDbContext _context;
        public ProfileController(AppDbContext appDbContext) { 
            _context = appDbContext;
        }
        public IActionResult Index()
        {
            var Porfile = _context.Users.ToList();
            return View(Porfile);
        }
    }
}
