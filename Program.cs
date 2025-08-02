using Capstone_Next_Step.Data;
using Microsoft.EntityFrameworkCore;

namespace Capstone_Next_Step
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddControllersWithViews(options =>
            {
                // Allow non-posted non-nullable reference type properties to be treated as optional
                // This prevents implicit "field is required" validation for properties not present in the form
                options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
            });
			builder.Services.AddSession(options =>
			{
				options.IdleTimeout = TimeSpan.FromMinutes(30);
				options.Cookie.HttpOnly = true;
				options.Cookie.IsEssential = true;
			});
			
			builder.Services.AddDbContext<AppDbContext>(options => 
				options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

			var app = builder.Build();

			// Configure the HTTP request pipeline
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseRouting();
			app.UseSession();
			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Login}/{action=AdminLogin}/{id?}");

            app.Run();
		}
	}
}
