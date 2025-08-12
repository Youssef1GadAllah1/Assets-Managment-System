using Microsoft.AspNetCore.Mvc;
using Capstone_Next_Step.Data;
using Capstone_Next_Step.Models;
using System;
using System.Linq;

namespace Capstone_Next_Step.Controllers
{
    public class ChatController : Controller
    {
        private readonly AppDbContext _context;

        public ChatController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SendMessage([FromBody] ChatMessageRequest request)
        {
            try
            {
                var raw = request.Message ?? string.Empty;
                string userMessage = raw.ToLower().Trim();
                string botResponse;

                if (string.IsNullOrWhiteSpace(userMessage))
                {
                    botResponse = "Please type your message.";
                }
                else if (userMessage.Contains("hello") || userMessage.Contains("hi"))
                {
                    botResponse = "Hello! How can I help you today?";
                }
                else if (userMessage.Contains("how are you"))
                {
                    botResponse = "I'm doing great, thank you! How can I assist you with asset management?";
                }
                else if (userMessage.Contains("problem") || userMessage.Contains("issue"))
                {
                    botResponse = "Sorry to hear you have an issue. Can you describe it in detail?";
                }
                else if (userMessage.Contains("thanks") || userMessage.Contains("thank you"))
                {
                    botResponse = "You're welcome! If you need anything else, feel free to ask.";
                }
                else if (userMessage.Contains("who are you"))
                {
                    botResponse = "I'm the smart assistant for the Asset Management System. Ask me anything about the system or your assets!";
                }
                else if (userMessage.Contains("time"))
                {
                    botResponse = $"Current time: {DateTime.Now:HH:mm:ss}";
                }
                else if (userMessage.Contains("date"))
                {
                    botResponse = $"Today's date: {DateTime.Now:dd/MM/yyyy}";
                }
                else if (userMessage.Contains("asset") || userMessage.Contains("assets"))
                {
                    botResponse = "You can manage assets from the Assets page, where you can add, edit, and delete them.";
                }
                else if (userMessage.Contains("report") || userMessage.Contains("reports"))
                {
                    botResponse = "You can create and view reports from the Reports page.";
                }
                else if (userMessage.Contains("user") || userMessage.Contains("employee"))
                {
                    botResponse = "You can manage users and employees from the Employees page.";
                }
                else
                {
                    if (userMessage.Contains("help"))
                    {
                        botResponse = "I can help with assets, reports, and users. Try asking about any of these.";
                    }
                    else if (userMessage.Contains("where") && userMessage.Contains("report"))
                    {
                        botResponse = "Go to the Reports page to add or view reports.";
                    }
                    else if (userMessage.Contains("password"))
                    {
                        botResponse = "Password changes are currently handled by the administrator.";
                    }
                    else
                    {
                        botResponse = $"You said: '{raw}'. I'm still learning. Ask me about assets, reports, users, or type 'help'.";
                    }
                }

                return Json(new
                {
                    success = true,
                    response = botResponse,
                    timestamp = DateTime.Now.ToString("HH:mm:ss")
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Chat error: {ex.Message}");
                return Json(new
                {
                    success = false,
                    response = "Sorry, an error occurred while processing your message.",
                    timestamp = DateTime.Now.ToString("HH:mm:ss")
                });
            }
        }
    }

    public class ChatMessageRequest
    {
        public string Message { get; set; }
    }
}
