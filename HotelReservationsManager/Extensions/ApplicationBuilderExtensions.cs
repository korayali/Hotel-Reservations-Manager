using HotelReservationsManager.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelReservationsManager.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static async Task SeedDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var services = scope.ServiceProvider;
            var environment = services.GetRequiredService<IWebHostEnvironment>();
            var db = services.GetRequiredService<HotelReservationsManagerDbContext>();
            var config = services.GetRequiredService<IConfiguration>();

            bool resetDb = config.GetValue<bool>("DatabaseSettings:ResetOnStartup");
            bool seedTestData = config.GetValue<bool>("DatabaseSettings:SeedTestData");

            // DEV ONLY → completely recreate database
            if (environment.IsDevelopment() && resetDb)
            {
                await db.Database.EnsureDeletedAsync();
                await db.Database.MigrateAsync();
            }

            // Always seed required system data
            var baseSeeder = services.GetRequiredService<DbSeeder>();
            await baseSeeder.InitialSeed();

            // DEV ONLY → seed test/demo data
            if (environment.IsDevelopment() && seedTestData)
            {
                await TestDbSeeder.SeedAsync(services);
            }

            // future: if you have more seeders, you can call them here
            // await services.GetRequiredService<AnotherSeeder>().Seed();
        }
    }
}
