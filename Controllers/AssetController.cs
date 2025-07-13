using Capstone_Next_Step.Data;
using Capstone_Next_Step.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Capstone_Next_Step.Controllers
{
    public class AssetController : Controller
    {
        private AppDbContext _context;
        public AssetController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public IActionResult Index()
        {
            var ProdectList = _context.Assets.ToList();
            return View(ProdectList);
        }

        public IActionResult AddAsset()
        {
            return View();
        }
        public IActionResult AddAssetsForm(Asset asset)
        {
            _context.Assets.Add(asset);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult AddProduct()
        {
           var p = _context.Products.ToList();
            return View(p);
        }
      
        public IActionResult AddProductForm(Product product)
        {
          
            

            _context.Products.Add(product);
            _context.SaveChanges();
                    
            
            return RedirectToAction("Inventory");
        }

        public IActionResult Inventory()
        {
            var Inventory = _context.Products.ToList();
            return View(Inventory);
        }
        public IActionResult ViewMap()
        {
            return View();
        }

        // Dictionary to map Egyptian cities to coordinates
        private readonly Dictionary<string, (double lat, double lng)> _egyptianCities = new()
        {
            { "cairo", (30.0444, 31.2357) },
            { "alexandria", (31.2001, 29.9187) },
            { "luxor", (25.6872, 32.6396) },
            { "aswan", (24.0889, 32.8998) },
            { "hurghada", (27.2579, 33.8116) },
            { "sharm el sheikh", (27.9158, 34.3300) },
            { "port said", (31.2653, 32.3019) },
            { "suez", (29.9668, 32.5498) },
            { "ismailia", (30.5965, 32.2715) },
            { "mansoura", (31.0409, 31.3785) },
            { "tanta", (30.7865, 31.0004) },
            { "zagazig", (30.5877, 31.5022) },
            { "fayoum", (29.3084, 30.8428) },
            { "beni suef", (29.0661, 31.0994) },
            { "minya", (28.0871, 30.7618) },
            { "asyut", (27.1783, 31.1859) },
            { "sohag", (26.5569, 31.6948) },
            { "qena", (26.1551, 32.7160) },
            { "kom ombo", (24.4761, 32.9420) },
            { "edfu", (24.9777, 32.8714) }
        };

        // Helper method to get coordinates from location string
        private (double lat, double lng) GetCoordinatesFromLocation(string location)
        {
            if (string.IsNullOrEmpty(location))
                return (26.8206, 30.8025); // Default to center of Egypt

            var locationKey = location.ToLower().Trim();

            if (_egyptianCities.ContainsKey(locationKey))
            {
                return _egyptianCities[locationKey];
            }

            // If location not found, try partial matching
            var partialMatch = _egyptianCities.FirstOrDefault(city =>
                city.Key.Contains(locationKey) || locationKey.Contains(city.Key));

            if (partialMatch.Key != null)
            {
                return partialMatch.Value;
            }

            // Default coordinates if location not recognized
            return (26.8206, 30.8025);
        }

        // New API endpoint to get assets as JSON for the map
        [HttpGet]
        public IActionResult GetAssets()
        {
            try
            {
                var assetsFromDb = _context.Assets.ToList();

                var assets = assetsFromDb.Select(a =>
                {
                    var coordinates = GetCoordinatesFromLocation(a.Location);
                    return new
                    {
                        id = a.Id,
                        name = a.Name,
                        description = $"{a.Category} - {a.Type} - {a.Color}",
                        location = new
                        {
                            lat = coordinates.lat,
                            lng = coordinates.lng
                        },
                        status = a.Status?.ToLower() ?? "available",
                        assignedTo = a.UserId, // Assuming User has Name property
                        lastUpdated = a.date.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                        category = a.Category,
                        type = a.Type,
                        price = a.Price,
                        color = a.Color,
                        locationName = a.Location,
                        imageUrl = a.ImageUrl
                    };
                }).ToList();

                return Json(assets);
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error fetching assets: {ex.Message}");
                return StatusCode(500, new { error = "Failed to fetch assets" });
            }
        }

        // API endpoint to update asset location (for mobile tracking)
        [HttpPost]
        public IActionResult UpdateAssetLocation([FromBody] UpdateLocationRequest request)
        {
            try
            {
                var asset = _context.Assets.Find(request.AssetId);
                if (asset == null)
                {
                    return NotFound(new { error = "Asset not found" });
                }

                asset.Location = request.LocationName;
                asset.date = DateTime.Now; // Update the date field as last updated

                _context.SaveChanges();

                return Ok(new { message = "Location updated successfully" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating asset location: {ex.Message}");
                return StatusCode(500, new { error = "Failed to update location" });
            }
        }
    }

    // DTO for location update requests
    public class UpdateLocationRequest
    {
        public int AssetId { get; set; }
        public string LocationName { get; set; }
    }
}