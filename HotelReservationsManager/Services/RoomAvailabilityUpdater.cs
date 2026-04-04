using HotelReservationsManager.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HotelReservationsManager.Services
{
    public class RoomAvailabilityUpdater : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly TimeSpan _updateInterval = TimeSpan.FromHours(1); // adjust as needed

        public RoomAvailabilityUpdater(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await UpdateRoomAvailabilityAsync();
                await Task.Delay(_updateInterval, stoppingToken);
            }
        }

        private async Task UpdateRoomAvailabilityAsync()
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<HotelReservationsManagerDbContext>();
            var today = DateTime.Today;

            var rooms = await context.Rooms.ToListAsync();

            foreach (var room in rooms)
            {
                room.IsFree = !await context.Reservations
                    .AnyAsync(r => r.RoomId == room.Id &&
                                   r.CheckInDate <= today &&
                                   r.CheckOutDate >= today);
            }

            await context.SaveChangesAsync();
        }
    }
}