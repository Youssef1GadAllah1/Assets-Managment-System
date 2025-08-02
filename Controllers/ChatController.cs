using Microsoft.AspNetCore.Mvc;
using Capstone_Next_Step.Data;
using Capstone_Next_Step.Models;

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
                string userMessage = raw.ToLower();
                string botResponse;

                // Validate input
                if (string.IsNullOrWhiteSpace(userMessage))
                {
                    botResponse = "يرجى كتابة رسالتك";
                }
                else if (userMessage.Contains("hello") || userMessage.Contains("hi") || userMessage.Contains("مرحبا"))
                {
                    botResponse = "مرحباً! كيف يمكنني مساعدتك اليوم؟";
                }
                else if (userMessage.Contains("how are you") || userMessage.Contains("عامل ايه") || userMessage.Contains("اخبارك"))
                {
                    botResponse = "أنا بخير، شكراً لك! كيف يمكنني مساعدتك في إدارة الأصول؟";
                }
                else if (userMessage.Contains("problem") || userMessage.Contains("مشكلة"))
                {
                    botResponse = "أعتذر لسماع أن لديك مشكلة. هل يمكنك وصفها بالتفصيل؟";
                }
                else if (userMessage.Contains("thanks") || userMessage.Contains("شكرا"))
                {
                    botResponse = "العفو! إذا احتجت أي شيء آخر، لا تتردد في السؤال.";
                }
                else if (userMessage.Contains("who are you") || userMessage.Contains("انت مين"))
                {
                    botResponse = "أنا المساعد الذكي لنظام إدارة الأصول. اسألني أي شيء عن النظام أو أصولك!";
                }
                else if (userMessage.Contains("time") || userMessage.Contains("الساعة"))
                {
                    botResponse = $"الوقت الحالي: {DateTime.Now.ToString("HH:mm:ss")}";
                }
                else if (userMessage.Contains("date") || userMessage.Contains("التاريخ"))
                {
                    botResponse = $"التاريخ الحالي: {DateTime.Now.ToString("dd/MM/yyyy")}";
                }
                else if (userMessage.Contains("asset") || userMessage.Contains("أصل") || userMessage.Contains("أصول"))
                {
                    botResponse = "يمكنك إدارة الأصول من خلال صفحة الأصول. هناك يمكنك إضافة وتعديل وحذف الأصول.";
                }
                else if (userMessage.Contains("report") || userMessage.Contains("تقرير") || userMessage.Contains("تقارير"))
                {
                    botResponse = "يمكنك إنشاء وعرض التقارير من صفحة التقارير. هناك يمكنك إضافة تقارير جديدة.";
                }
                else if (userMessage.Contains("user") || userMessage.Contains("مستخدم") || userMessage.Contains("موظف"))
                {
                    botResponse = "يمكنك إدارة المستخدمين والموظفين من صفحة الموظفين.";
                }
                else
                {
                    // Simple multilingual fallback using keyword intents
                    // Try to detect common intents dynamically
                    if (userMessage.Contains("help") || userMessage.Contains("مساعدة") || userMessage.Contains("ازاي"))
                    {
                        botResponse = "I can help with assets, reports, and users. يمكنك سؤالي عن الأصول، التقارير والمستخدمين.";
                    }
                    else if (userMessage.Contains("where") && (userMessage.Contains("report") || userMessage.Contains("تقرير")))
                    {
                        botResponse = "Open Reports page to add or view reports. افتح صفحة التقارير لإضافة أو عرض التقارير.";
                    }
                    else if (userMessage.Contains("password") || userMessage.Contains("باسورد") || userMessage.Contains("كلمة السر"))
                    {
                        botResponse = "Password changes are handled by admin for now. تغيير كلمة السر من خلال المشرف حالياً.";
                    }
                    else
                    {
                        // Echo back with a generic helpful response (English + Arabic)
                        botResponse = $"You said: '{raw}'. I'm learning. Ask me about assets, reports, users or help. قلت: '{raw}'. ما زلت أتعلم، اسألني عن الأصول أو التقارير أو المستخدمين أو اكتب 'help / مساعدة'.";
                    }
                }

                return Json(new { 
                    success = true, 
                    response = botResponse,
                    timestamp = DateTime.Now.ToString("HH:mm:ss")
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Chat error: {ex.Message}");
                return Json(new { 
                    success = false, 
                    response = "عذراً، حدث خطأ في معالجة رسالتك",
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
