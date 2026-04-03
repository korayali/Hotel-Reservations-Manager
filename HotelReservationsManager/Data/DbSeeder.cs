using HotelReservationsManager.Models.Domains;
using Microsoft.AspNetCore.Identity;

namespace HotelReservationsManager.Data
{
    public class DbSeeder
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<User> userManager;

        public DbSeeder(
            RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public async Task InitialSeed()
        {
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (!await roleManager.RoleExistsAsync("Employee"))
            {
                await roleManager.CreateAsync(new IdentityRole("Employee"));
            }

            var adminEmail = "admin@example.com";

            var existingUser = await userManager.FindByEmailAsync(adminEmail);

            if (existingUser == null)
            {
                var adminUser = new User
                {
                    DisplayName = "Admin",
                    FirstName = "Admin",
                    MiddleName = "Adminov",
                    LastName = "Adminov",
                    EGN = "0000000000",
                    HireDate = DateOnly.FromDateTime(DateTime.UtcNow),
                    IsActive = true,
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, "Admin123!");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}
