using Capstone_Next_Step.Data;
using Microsoft.AspNetCore.Mvc;

namespace Capstone_Next_Step.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _context;

        public LoginController(AppDbContext context)
        {
            _context = context;
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AdminCheckLogin(string UserNameInput, string PasswordInput)
        {
            try
            {
                if (string.IsNullOrEmpty(UserNameInput) || string.IsNullOrEmpty(PasswordInput))
                {
                    ViewBag.Error = "Please enter your username and password.";
                    return View("AdminLogin");
                }

                var admin = _context.Admins.FirstOrDefault(m => 
                    m.UserName == UserNameInput && 
                    m.Password == PasswordInput);

                if (admin == null)
                {
                    ViewBag.Error = "Incorrect username or password";
                    return View("AdminLogin");
                }

                // Set session data
                HttpContext.Session.SetString("UserName", admin.UserName);
                HttpContext.Session.SetString("Name", admin.Name);
                HttpContext.Session.SetString("Role", "Admin");

                return Redirect("/Home/Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Admin login error: {ex.Message}");
                ViewBag.Error = "An error occurred while logging in.";
                return View("AdminLogin");
            }
        }

        [HttpPost]
        public IActionResult CheckLogin(string UserNameInput, string PasswordInput)
        {
            try
            {
                if (string.IsNullOrEmpty(UserNameInput) || string.IsNullOrEmpty(PasswordInput))
                {
                    ViewBag.Error = "Please enter your username and password.";
                    return View("Login");
                }

                var user = _context.Users.FirstOrDefault(m => 
                    m.UserName == UserNameInput && 
                    m.Password == PasswordInput);

                if (user == null)
                {
                    ViewBag.Error = "Incorrect username or password";
                    return View("Login");
                }

                // Set session data
                HttpContext.Session.SetString("UserName", user.UserName);
                HttpContext.Session.SetString("Name", user.Name);
                HttpContext.Session.SetString("Role", user.Role ?? "User");

                return Redirect("/Home/Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"User login error: {ex.Message}");
                ViewBag.Error = "An error occurred while logging in.";
                return View("Login");
            }
        }

        public IActionResult Logout()
        {
            try
            {
                HttpContext.Session.Clear();
                return RedirectToAction("AdminLogin");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Logout error: {ex.Message}");
                return RedirectToAction("AdminLogin");
            }
        }
    }
}
