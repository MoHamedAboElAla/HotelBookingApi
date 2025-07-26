using HotelBookingApi.Models;
using Microsoft.AspNetCore.Identity;

namespace HotelBookingApi.Seeder
{
    public static class Seeder
    {
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roles = { "Admin", "Client" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                    Console.WriteLine($"✅ Role '{role}' created.");
                }
            }

            var adminEmail = "admin@hotel.com";
            var existingAdmin = await userManager.FindByEmailAsync(adminEmail);

            if (existingAdmin == null)
            {
                var newAdmin = new ApplicationUser
                {
                    UserName = "admin",
                    Email = adminEmail,
                    Name = "Super Admin"
                };

                var result = await userManager.CreateAsync(newAdmin, "Admin@123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                    Console.WriteLine("✅ Admin user created and assigned to 'Admin' role.");
                }
                else
                {
                    Console.WriteLine("❌ Failed to create admin user:");
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"   - {error.Description}");
                    }
                }
            }
            else
            {
                Console.WriteLine("ℹ️ Admin user already exists.");
            }
        }
    }
}