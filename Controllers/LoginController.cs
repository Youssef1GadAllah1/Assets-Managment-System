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
                    ViewBag.Error = "يرجى إدخال اسم المستخدم وكلمة المرور";
                    return View("AdminLogin");
                }

                var admin = _context.Admins.FirstOrDefault(m => 
                    m.UserName == UserNameInput && 
                    m.Password == PasswordInput);

                if (admin == null)
                {
                    ViewBag.Error = "اسم المستخدم أو كلمة المرور غير صحيحة";
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
                ViewBag.Error = "حدث خطأ أثناء تسجيل الدخول";
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
                    ViewBag.Error = "يرجى إدخال اسم المستخدم وكلمة المرور";
                    return View("Login");
                }

                var user = _context.Users.FirstOrDefault(m => 
                    m.UserName == UserNameInput && 
                    m.Password == PasswordInput);

                if (user == null)
                {
                    ViewBag.Error = "اسم المستخدم أو كلمة المرور غير صحيحة";
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
                ViewBag.Error = "حدث خطأ أثناء تسجيل الدخول";
                return View("Login");
            }
        }

        public IActionResult Logout()
        {
            try
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Logout error: {ex.Message}");
                return RedirectToAction("Login");
            }
        }
    }
}
