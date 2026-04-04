using Forked.Services;
using HotelReservationsManager.Data;
using HotelReservationsManager.Models.Domains;
using HotelReservationsManager.Services;
using HotelReservationsManager.Services.Guests;
using HotelReservationsManager.Services.Interfaces;
using HotelReservationsManager.Services.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
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
            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
            })
            .AddEntityFrameworkStores<HotelReservationsManagerDbContext>()
            .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
            });
        }

        public static void AddDbSeeder(this IServiceCollection services)
        {
            services.AddScoped<DbSeeder>();
        }

        public static void AddEmailServices(this IServiceCollection services)
        {
            services.AddTransient<IEmailSender, EmailSender>();
        }

        public static void AddUserService(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<SignInManager<User>, HotelReservationsManagerSignInManager>();
        }

        public static void AddGuestService(this IServiceCollection services)
        {
            services.AddScoped<IGuestService, GuestService>();
        }

        public static void AddRoomService(this IServiceCollection services)
        {
            services.AddScoped<IRoomService, RoomService>();
        }
        public static void AddReservationService(this IServiceCollection services)
        {
            services.AddScoped<IReservationService, ReservationService>();
        }
    }
}
