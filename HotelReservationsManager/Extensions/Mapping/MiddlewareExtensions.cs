using HotelReservationsManager.Middleware;

namespace HotelReservationsManager.Extensions.Mapping
{
    public static class MiddlewareExtensions
    {
        public static void UseActiveUserCheck(this WebApplication app)
        {
            app.UseMiddleware<ActiveUserMiddleware>();
        }
    }
}
