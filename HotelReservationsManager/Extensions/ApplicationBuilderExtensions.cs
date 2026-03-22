using HotelReservationsManager.Data;

namespace HotelReservationsManager.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static async Task SeedDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var seeder = scope.ServiceProvider.GetRequiredService<DbSeeder>();

            await seeder.InitialSeed();

            // future: if you have more seeders, you can call them here
            // await services.GetRequiredService<AnotherSeeder>().Seed();
        }
    }
}
