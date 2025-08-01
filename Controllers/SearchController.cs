using Capstone_Next_Step.Data;
using Capstone_Next_Step.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Capstone_Next_Step.Controllers
{
    public class SearchController : Controller
    {
        private readonly AppDbContext _context;

        public SearchController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index(string q)
        {
            try
            {
                if (string.IsNullOrEmpty(q))
                {
                    return View(new SearchResultViewModel());
                }

                var searchTerm = q.ToLower();
                var results = new SearchResultViewModel
                {
                    SearchTerm = q,
                    Assets = _context.Assets
                        .Where(a => (a.Name != null && a.Name.ToLower().Contains(searchTerm)) || 
                                   (a.Category != null && a.Category.ToLower().Contains(searchTerm)) ||
                                   (a.Type != null && a.Type.ToLower().Contains(searchTerm)) ||
                                   (a.Location != null && a.Location.ToLower().Contains(searchTerm)))
                        .Take(10)
                        .ToList(),
                    Products = _context.Products
                        .Where(p => (p.Name != null && p.Name.ToLower().Contains(searchTerm)) ||
                                   (p.Category != null && p.Category.ToLower().Contains(searchTerm)) ||
                                   (p.Type != null && p.Type.ToLower().Contains(searchTerm)))
                        .Take(10)
                        .ToList(),
                    Users = _context.Users
                        .Where(u => (u.Name != null && u.Name.ToLower().Contains(searchTerm)) ||
                                   (u.UserName != null && u.UserName.ToLower().Contains(searchTerm)) ||
                                   (u.JobTitle != null && u.JobTitle.ToLower().Contains(searchTerm)) ||
                                   (u.Email != null && u.Email.ToLower().Contains(searchTerm)))
                        .Take(10)
                        .ToList(),
                    Reports = _context.Reports
                        .Where(r => (r.Title != null && r.Title.ToLower().Contains(searchTerm)) ||
                                   (r.Description != null && r.Description.ToLower().Contains(searchTerm)) ||
                                   (r.Author != null && r.Author.ToLower().Contains(searchTerm)))
                        .Take(10)
                        .ToList()
                };

                return View(results);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Search error: {ex.Message}");
                return View(new SearchResultViewModel { SearchTerm = q });
            }
        }

        [HttpGet]
        public IActionResult QuickSearch(string term)
        {
            try
            {
                if (string.IsNullOrEmpty(term))
                {
                    return Json(new { results = new List<object>() });
                }

                var searchTerm = term.ToLower();
                var results = new List<object>();

                // Search in Assets
                var assets = _context.Assets
                    .Where(a => (a.Name != null && a.Name.ToLower().Contains(searchTerm)) || 
                               (a.Category != null && a.Category.ToLower().Contains(searchTerm)))
                    .Take(5)
                    .Select(a => new { 
                        type = "Asset", 
                        title = a.Name ?? "Unknown", 
                        subtitle = a.Category ?? "Unknown",
                        url = $"/Asset/Index",
                        icon = "fas fa-server"
                    });

                // Search in Products
                var products = _context.Products
                    .Where(p => (p.Name != null && p.Name.ToLower().Contains(searchTerm)) ||
                               (p.Category != null && p.Category.ToLower().Contains(searchTerm)))
                    .Take(5)
                    .Select(p => new { 
                        type = "Product", 
                        title = p.Name ?? "Unknown", 
                        subtitle = p.Category ?? "Unknown",
                        url = $"/Asset/Inventory",
                        icon = "fas fa-box"
                    });

                // Search in Users
                var users = _context.Users
                    .Where(u => (u.Name != null && u.Name.ToLower().Contains(searchTerm)) ||
                               (u.JobTitle != null && u.JobTitle.ToLower().Contains(searchTerm)))
                    .Take(5)
                    .Select(u => new { 
                        type = "User", 
                        title = u.Name ?? "Unknown", 
                        subtitle = u.JobTitle ?? "Unknown",
                        url = $"/Home/Employee",
                        icon = "fas fa-user"
                    });

                results.AddRange(assets);
                results.AddRange(products);
                results.AddRange(users);

                return Json(new { results = results.Take(10) });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"QuickSearch error: {ex.Message}");
                return Json(new { results = new List<object>(), error = ex.Message });
            }
        }
    }
} 