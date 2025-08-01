using System.ComponentModel.DataAnnotations;

namespace Capstone_Next_Step.Models
{
    public class SearchResultViewModel
    {
        public string SearchTerm { get; set; } = "";
        
        public List<Asset> Assets { get; set; } = new List<Asset>();
        
        public List<Product> Products { get; set; } = new List<Product>();
        
        public List<User> Users { get; set; } = new List<User>();
        
        public List<Report> Reports { get; set; } = new List<Report>();
        
        public int TotalResults => Assets.Count + Products.Count + Users.Count + Reports.Count;
        
        public bool HasResults => TotalResults > 0;
        
        public int AssetsCount => Assets.Count;
        public int ProductsCount => Products.Count;
        public int UsersCount => Users.Count;
        public int ReportsCount => Reports.Count;
    }
} 