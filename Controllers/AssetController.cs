using Capstone_Next_Step.Data;
using Capstone_Next_Step.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Capstone_Next_Step.Controllers
{
    public class AssetController : Controller
    {
        private readonly AppDbContext _context;
        
        public AssetController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public IActionResult Index()
        {
            try
            {
                var assets = _context.Assets
                    .OrderByDescending(a => a.date)
                    .ToList();
                    
                return View(assets);
            }
            catch (Exception ex)
            {
                // Log error in production
                Console.WriteLine($"Error in Asset/Index: {ex.Message}");
                return View(new List<Asset>());
            }
        }

        public IActionResult AddAsset()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddAssetsForm(Asset asset, double? latitude, double? longitude)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("AddAsset", asset);
                }

                // Set current date if not provided
                if (asset.date == default)
                {
                    asset.date = DateTime.Now;
                }

                // Handle custom coordinates
                if (latitude.HasValue && longitude.HasValue)
                {
                    asset.Location = $"{asset.Location}|{latitude.Value:F6},{longitude.Value:F6}";
                }

                _context.Assets.Add(asset);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "تم إضافة الأصل بنجاح";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding asset: {ex.Message}");
                ModelState.AddModelError("", "حدث خطأ أثناء إضافة الأصل");
                return View("AddAsset", asset);
            }
        }

        public IActionResult AddProduct()
        {
            try
            {
                var products = _context.Products
                    .OrderByDescending(p => p.Id)
                    .ToList();
                    
                return View(products);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddProduct: {ex.Message}");
                return View(new List<Product>());
            }
        }

        [HttpPost]
        public IActionResult AddProductForm(Product product)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("AddProduct", product);
                }

                _context.Products.Add(product);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "تم إضافة المنتج بنجاح";
                return RedirectToAction("Inventory");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding product: {ex.Message}");
                ModelState.AddModelError("", "حدث خطأ أثناء إضافة المنتج");
                return View("AddProduct", product);
            }
        }

        public IActionResult Inventory()
        {
            try
            {
                var inventory = _context.Products
                    .OrderByDescending(p => p.Id)
                    .ToList();
                    
                return View(inventory);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Asset/Inventory: {ex.Message}");
                return View(new List<Product>());
            }
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
            { "edfu", (24.9777, 32.8714) },
            { "giza", (30.0131, 31.2089) },
            { "6 october", (29.9697, 30.9564) },
            { "new cairo", (30.0300, 31.4700) },
            { "shoubra", (30.0778, 31.2428) },
            { "helwan", (29.8419, 31.3344) },
            { "maadi", (29.9633, 31.2719) },
            { "zamalek", (30.0589, 31.2239) },
            { "dokki", (30.0381, 31.2097) },
            { "mohandessin", (30.0589, 31.2097) },
            { "agouza", (30.0589, 31.2097) },
            { "nasr city", (30.0500, 31.4700) },
            { "abbassia", (30.0500, 31.2700) },
            { "ain shams", (30.1300, 31.3300) },
            { "shoubra el kheima", (30.1300, 31.2400) },
            { "el marg", (30.1500, 31.3500) },
            { "badr city", (30.1300, 31.7400) },
            { "obour city", (30.2000, 31.4700) },
            { "shorouk city", (30.1500, 31.6200) },
            { "rehab city", (30.0300, 31.4700) },
            { "madinaty", (30.0800, 31.4700) },
            { "tagammu el khamis", (30.0500, 31.4700) },
            { "tagammu el awal", (30.0500, 31.4700) },
            { "tagammu el thani", (30.0500, 31.4700) },
            { "tagammu el thaleth", (30.0500, 31.4700) },
            { "tagammu el rabe3", (30.0500, 31.4700) },
            { "tagammu el sadis", (30.0500, 31.4700) },
            { "tagammu el sabi3", (30.0500, 31.4700) },
            { "tagammu el thamin", (30.0500, 31.4700) },
            { "tagammu el tasi3", (30.0500, 31.4700) },
            { "tagammu el 3ashir", (30.0500, 31.4700) }
        };

        // Helper method to get coordinates from location string
        private (double lat, double lng) GetCoordinatesFromLocation(string location)
        {
            if (string.IsNullOrEmpty(location))
                return (30.0444, 31.2357); // Default to Cairo

            // Check if location contains custom coordinates (format: "city|lat,lng")
            if (location.Contains("|"))
            {
                var parts = location.Split('|');
                if (parts.Length == 2)
                {
                    var cityName = parts[0].Trim();
                    var coordinates = parts[1].Trim();
                    
                    if (coordinates.Contains(","))
                    {
                        var coordParts = coordinates.Split(',');
                        if (coordParts.Length == 2 && 
                            double.TryParse(coordParts[0], out double lat) && 
                            double.TryParse(coordParts[1], out double lng))
                        {
                            return (lat, lng);
                        }
                    }
                    
                    // If coordinates parsing failed, use city name
                    location = cityName;
                }
            }

            var locationKey = location.ToLower().Trim();

            // Direct match
            if (_egyptianCities.ContainsKey(locationKey))
            {
                return _egyptianCities[locationKey];
            }

            // Handle Arabic city names
            var arabicToEnglish = new Dictionary<string, string>
            {
                { "القاهرة", "cairo" },
                { "الاسكندرية", "alexandria" },
                { "الجيزة", "giza" },
                { "ستة اكتوبر", "6 october" },
                { "القاهرة الجديدة", "new cairo" },
                { "مدينة نصر", "nasr city" },
                { "العباسية", "abbassia" },
                { "عين شمس", "ain shams" },
                { "شبرا", "shoubra" },
                { "شبرا الخيمة", "shoubra el kheima" },
                { "المرج", "el marg" },
                { "حلوان", "helwan" },
                { "المعادي", "maadi" },
                { "الزمالك", "zamalek" },
                { "الدقي", "dokki" },
                { "المهندسين", "mohandessin" },
                { "العجوزة", "agouza" },
                { "مدينة بدر", "badr city" },
                { "مدينة العبور", "obour city" },
                { "مدينة الشروق", "shorouk city" },
                { "مدينة الرحاب", "rehab city" },
                { "مدينتي", "madinaty" },
                { "التجمع الخامس", "tagammu el khamis" },
                { "التجمع الاول", "tagammu el awal" },
                { "التجمع الثاني", "tagammu el thani" },
                { "التجمع الثالث", "tagammu el thaleth" },
                { "التجمع الرابع", "tagammu el rabe3" },
                { "التجمع السادس", "tagammu el sadis" },
                { "التجمع السابع", "tagammu el sabi3" },
                { "التجمع الثامن", "tagammu el thamin" },
                { "التجمع التاسع", "tagammu el tasi3" },
                { "التجمع العاشر", "tagammu el 3ashir" },
                { "الاقصر", "luxor" },
                { "اسوان", "aswan" },
                { "الغردقة", "hurghada" },
                { "شرم الشيخ", "sharm el sheikh" },
                { "بورسعيد", "port said" },
                { "السويس", "suez" },
                { "الاسماعيلية", "ismailia" },
                { "المنصورة", "mansoura" },
                { "طنطا", "tanta" },
                { "الزقازيق", "zagazig" },
                { "الفيوم", "fayoum" },
                { "بني سويف", "beni suef" },
                { "المنيا", "minya" },
                { "اسيوط", "asyut" },
                { "سوهاج", "sohag" },
                { "قنا", "qena" },
                { "كوم امبو", "kom ombo" },
                { "ادفو", "edfu" }
            };

            if (arabicToEnglish.ContainsKey(locationKey))
            {
                var englishName = arabicToEnglish[locationKey];
                if (_egyptianCities.ContainsKey(englishName))
                {
                    return _egyptianCities[englishName];
                }
            }

            // Partial matching for English names
            var partialMatch = _egyptianCities.FirstOrDefault(city =>
                city.Key.Contains(locationKey) || locationKey.Contains(city.Key));

            if (partialMatch.Key != null)
            {
                return partialMatch.Value;
            }

            // Partial matching for Arabic names
            foreach (var arabicEntry in arabicToEnglish)
            {
                if (arabicEntry.Key.Contains(locationKey) || locationKey.Contains(arabicEntry.Key))
                {
                    var englishName = arabicEntry.Value;
                    if (_egyptianCities.ContainsKey(englishName))
                    {
                        return _egyptianCities[englishName];
                    }
                }
            }

            // Default coordinates if location not recognized
            return (30.0444, 31.2357); // Default to Cairo
        }

        // New API endpoint to get assets as JSON for the map
        [HttpGet]
        public IActionResult GetAssets()
        {
            try
            {
                var assetsFromDb = _context.Assets.ToList();
                Console.WriteLine($"Found {assetsFromDb.Count} assets in database");

                var assets = assetsFromDb.Select(a =>
                {
                    var coordinates = GetCoordinatesFromLocation(a.Location);
                    var locationName = GetLocationDisplayName(a.Location);
                    Console.WriteLine($"Asset '{a.Name}' at location '{a.Location}' mapped to coordinates: {coordinates.lat}, {coordinates.lng}");
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
                        locationName = locationName,
                        imageUrl = a.ImageUrl
                    };
                }).ToList();

                Console.WriteLine($"Successfully processed {assets.Count} assets for map display");
                return Json(assets);
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error fetching assets: {ex.Message}");
                return StatusCode(500, new { error = "Failed to fetch assets", details = ex.Message });
            }
        }

        // Helper method to get display name for location
        private string GetLocationDisplayName(string location)
        {
            if (string.IsNullOrEmpty(location))
                return "القاهرة";

            // If location contains custom coordinates, extract city name
            if (location.Contains("|"))
            {
                var parts = location.Split('|');
                if (parts.Length == 2)
                {
                    location = parts[0].Trim();
                }
            }

            var locationKey = location.ToLower().Trim();

            // Arabic to English mapping for display
            var arabicToEnglish = new Dictionary<string, string>
            {
                { "القاهرة", "cairo" },
                { "الاسكندرية", "alexandria" },
                { "الجيزة", "giza" },
                { "ستة اكتوبر", "6 october" },
                { "القاهرة الجديدة", "new cairo" },
                { "مدينة نصر", "nasr city" },
                { "العباسية", "abbassia" },
                { "عين شمس", "ain shams" },
                { "شبرا", "shoubra" },
                { "شبرا الخيمة", "shoubra el kheima" },
                { "المرج", "el marg" },
                { "حلوان", "helwan" },
                { "المعادي", "maadi" },
                { "الزمالك", "zamalek" },
                { "الدقي", "dokki" },
                { "المهندسين", "mohandessin" },
                { "العجوزة", "agouza" },
                { "مدينة بدر", "badr city" },
                { "مدينة العبور", "obour city" },
                { "مدينة الشروق", "shorouk city" },
                { "مدينة الرحاب", "rehab city" },
                { "مدينتي", "madinaty" },
                { "التجمع الخامس", "tagammu el khamis" },
                { "التجمع الاول", "tagammu el awal" },
                { "التجمع الثاني", "tagammu el thani" },
                { "التجمع الثالث", "tagammu el thaleth" },
                { "التجمع الرابع", "tagammu el rabe3" },
                { "التجمع السادس", "tagammu el sadis" },
                { "التجمع السابع", "tagammu el sabi3" },
                { "التجمع الثامن", "tagammu el thamin" },
                { "التجمع التاسع", "tagammu el tasi3" },
                { "التجمع العاشر", "tagammu el 3ashir" },
                { "الاقصر", "luxor" },
                { "اسوان", "aswan" },
                { "الغردقة", "hurghada" },
                { "شرم الشيخ", "sharm el sheikh" },
                { "بورسعيد", "port said" },
                { "السويس", "suez" },
                { "الاسماعيلية", "ismailia" },
                { "المنصورة", "mansoura" },
                { "طنطا", "tanta" },
                { "الزقازيق", "zagazig" },
                { "الفيوم", "fayoum" },
                { "بني سويف", "beni suef" },
                { "المنيا", "minya" },
                { "اسيوط", "asyut" },
                { "سوهاج", "sohag" },
                { "قنا", "qena" },
                { "كوم امبو", "kom ombo" },
                { "ادفو", "edfu" }
            };

            // English to Arabic display mapping
            var englishToArabic = new Dictionary<string, string>
            {
                { "cairo", "القاهرة" },
                { "alexandria", "الإسكندرية" },
                { "giza", "الجيزة" },
                { "6 october", "6 أكتوبر" },
                { "new cairo", "القاهرة الجديدة" },
                { "nasr city", "مدينة نصر" },
                { "abbassia", "العباسية" },
                { "ain shams", "عين شمس" },
                { "shoubra", "شبرا" },
                { "shoubra el kheima", "شبرا الخيمة" },
                { "el marg", "المرج" },
                { "helwan", "حلوان" },
                { "maadi", "المعادي" },
                { "zamalek", "الزمالك" },
                { "dokki", "الدقي" },
                { "mohandessin", "المهندسين" },
                { "agouza", "العجوزة" },
                { "badr city", "مدينة بدر" },
                { "obour city", "مدينة العبور" },
                { "shorouk city", "مدينة الشروق" },
                { "rehab city", "مدينة الرحاب" },
                { "madinaty", "مدينتي" },
                { "tagammu el khamis", "التجمع الخامس" },
                { "tagammu el awal", "التجمع الأول" },
                { "tagammu el thani", "التجمع الثاني" },
                { "tagammu el thaleth", "التجمع الثالث" },
                { "tagammu el rabe3", "التجمع الرابع" },
                { "tagammu el sadis", "التجمع السادس" },
                { "tagammu el sabi3", "التجمع السابع" },
                { "tagammu el thamin", "التجمع الثامن" },
                { "tagammu el tasi3", "التجمع التاسع" },
                { "tagammu el 3ashir", "التجمع العاشر" },
                { "luxor", "الأقصر" },
                { "aswan", "أسوان" },
                { "hurghada", "الغردقة" },
                { "sharm el sheikh", "شرم الشيخ" },
                { "port said", "بورسعيد" },
                { "suez", "السويس" },
                { "ismailia", "الإسماعيلية" },
                { "mansoura", "المنصورة" },
                { "tanta", "طنطا" },
                { "zagazig", "الزقازيق" },
                { "fayoum", "الفيوم" },
                { "beni suef", "بني سويف" },
                { "minya", "المنيا" },
                { "asyut", "أسيوط" },
                { "sohag", "سوهاج" },
                { "qena", "قنا" },
                { "kom ombo", "كوم أمبو" },
                { "edfu", "إدفو" }
            };

            // Try to get Arabic display name
            if (englishToArabic.ContainsKey(locationKey))
            {
                return englishToArabic[locationKey];
            }

            // If not found, return the original location
            return location;
        }

        // API endpoint to update asset location (for mobile tracking)
        [HttpPost]
        public IActionResult UpdateAssetLocation([FromBody] UpdateLocationRequest request)
        {
            try
            {
                var asset = _context.Assets.FirstOrDefault(a => a.Id == request.AssetId);
                if (asset == null)
                {
                    return NotFound(new { error = "Asset not found" });
                }

                asset.Location = request.LocationName;
                _context.SaveChanges();

                return Json(new { success = true, message = "Location updated successfully" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating asset location: {ex.Message}");
                return StatusCode(500, new { error = "Failed to update location" });
            }
        }
    }

    public class UpdateLocationRequest
    {
        public int AssetId { get; set; }
        public string LocationName { get; set; }
    }
}