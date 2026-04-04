using HotelReservationsManager.Data;
using HotelReservationsManager.Models.ViewModels.Room;

namespace HotelReservationsManager.Services.RoomService
{
    public class RoomService: IRoomService
    {
        
        private readonly HotelReservationsManagerDbContext _context;

        public RoomService(HotelReservationsManagerDbContext context)
        {
            _context = context;
        }

        public async Task<RoomListViewModel> GetRoomsAsync(RoomFilterViewModel filters, int page, int pageSize)
        {
            var query = _context.Rooms.AsQueryable();

            if (filters.Type.HasValue)
                query = query.Where(r => r.Type == filters.Type.Value);

            switch (filters.AvailabilityFilter)
            {
                case RoomAvailabilityFilter.AvailableOnly:
                    query = query.Where(r => r.IsFree);
                    break;

                case RoomAvailabilityFilter.OccupiedOnly:
                    query = query.Where(r => !r.IsFree);
                    break;
            }

            if (filters.MinCapacity.HasValue)
                query = query.Where(r => r.Capacity >= filters.MinCapacity.Value);

            if (filters.MaxCapacity.HasValue)
                query = query.Where(r => r.Capacity <= filters.MaxCapacity.Value);

            int totalCount = await query.CountAsync();

            var rooms = await query
                .OrderBy(r => r.RoomNumber)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var cards = rooms.Select(r => new RoomCardViewModel
            {
                Id = r.Id,
                RoomNumber = r.RoomNumber,
                Type = r.Type,
                Capacity = r.Capacity,
                IsFree = r.IsFree,
                PricePerAdult = r.PricePerAdult,
                PricePerChild = r.PricePerChild
            }).ToList();

            return new RoomListViewModel(cards, page, pageSize, totalCount)
            {
                Filters = filters
            };
        }

        public async Task<DetailsRoomViewModel?> GetRoomDetailsAsync(int id)
        {
            var room = await _context.Rooms
                .Include(r => r.Reservations)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (room == null)
                return null;

            return new DetailsRoomViewModel
            {
                Id = room.Id,
                RoomNumber = room.RoomNumber,
                Type = room.Type,
                Capacity = room.Capacity,
                IsFree = room.IsFree,
                PricePerAdult = room.PricePerAdult,
                PricePerChild = room.PricePerChild,
                Reservations = room.Reservations.ToList()
            };
        }

        public async Task UpdateRoomAsync(DetailsRoomViewModel vm)
        {
            var room = await _context.Rooms.FindAsync(vm.Id)
                ?? throw new InvalidOperationException($"Room '{vm.Id}' not found.");

            room.RoomNumber = vm.RoomNumber;
            room.Type = vm.Type;
            room.Capacity = vm.Capacity;
            room.IsFree = vm.IsFree;
            room.PricePerAdult = vm.PricePerAdult;
            room.PricePerChild = vm.PricePerChild;

            await _context.SaveChangesAsync();
        }
    }
}
