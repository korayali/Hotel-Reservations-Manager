using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HotelReservationsManager.Data
{
    public class HotelReservationsManagerDbContextFactory : IDesignTimeDbContextFactory<HotelReservationsManagerDbContext>
    {
        public HotelReservationsManagerDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<HotelReservationsManagerDbContext>();

            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            return new HotelReservationsManagerDbContext(optionsBuilder.Options);
        }
    }
}
