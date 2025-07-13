using Microsoft.AspNetCore.Mvc;

namespace Capstone_Next_Step.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
