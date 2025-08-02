using Capstone_Next_Step.Models;
using Microsoft.EntityFrameworkCore;

namespace Capstone_Next_Step.Data
{
    public class AppDbContext:DbContext
    {


        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Additional configurations if needed
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<DTask> DTasks { get; set; }  
        public DbSet<Product> Products { get; set; }
        public DbSet<Notification> Notifications { get; set; }
       
    }
}
