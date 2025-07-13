using Capstone_Next_Step.Data;
using Microsoft.AspNetCore.Mvc;

namespace Capstone_Next_Step.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }
        private readonly AppDbContext _context; // ✅ Use Asset (your DbContext)

        public LoginController(AppDbContext context) // ✅ Fix constructor
        {
            _context = context;
        }
        public ActionResult AdminLogin() {
            return View();
        }

        public IActionResult AdminCheckLogin(string UserNameInput, string PasswordInput)
        {
            //check password and username
            var response = _context.Admins.FirstOrDefault(m => m.UserName == UserNameInput && m.Password == PasswordInput);

            // if username and password not currect
            if (response == null)
            {
                return RedirectToAction("Index");

            }
            //if username and password currect
            else
            {

                return Redirect("/Home/Index");
            }
        }
        public IActionResult CheckLogin(string UserNameInput, string PasswordInput)
        {
            //check password and username
            var response = _context.Users.FirstOrDefault(m => m.UserName == UserNameInput && m.Password == PasswordInput);

            // if username and password not currect
            if (response == null)
            {
                return RedirectToAction("Index");

            }
            //if username and password currect
            else
            {

                return Redirect("/Home/Index");
            }
        }
    }
}
