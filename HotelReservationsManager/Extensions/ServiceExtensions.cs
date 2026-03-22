using HotelReservationsManager.Data;
using HotelReservationsManager.Models.Domains;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace HotelReservationsManager.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddDatabase(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<HotelReservationsManagerDbContext>(options =>
                options.UseSqlServer(
                    config.GetConnectionString("DefaultConnection")));
        }

        public static void AddIdentityServices(this IServiceCollection services)
        {
            services.AddIdentityCore<User>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<HotelReservationsManagerDbContext>()
            .AddDefaultTokenProviders();
        }

        public static void AddDbSeeder(this IServiceCollection services)
        {
            services.AddScoped<DbSeeder>();
        }
    }
}
