using HotelReservationsManager.Data;
using HotelReservationsManager.Extensions.Mapping;
using HotelReservationsManager.Models;
using HotelReservationsManager.Models.ViewModels.Room;
using HotelReservationsManager.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelReservationsManager.Services
{
    public class RoomService : IRoomService
    {
        private readonly HotelReservationsManagerDbContext _context;

        public RoomService(HotelReservationsManagerDbContext context)
        {
            _context = context;
        }

        public async Task<RoomListViewModel> GetAllAsync(RoomFilterViewModel filters, int page, int pageSize)
        {
            var query = _context.Rooms.AsNoTracking();

            if (filters.Type.HasValue)
                query = query.Where(r => r.Type == filters.Type.Value);

            query = filters.AvailabilityFilter switch
            {
                RoomAvailabilityFilter.AvailableOnly => query.Where(r => r.IsFree),
                RoomAvailabilityFilter.OccupiedOnly => query.Where(r => !r.IsFree),
                _ => query
            };

            if (filters.MinCapacity.HasValue)
                query = query.Where(r => r.Capacity >= filters.MinCapacity.Value);

            if (filters.MaxCapacity.HasValue)
                query = query.Where(r => r.Capacity <= filters.MaxCapacity.Value);

            int totalItems = await query.CountAsync();

            var rooms = await query
                .OrderBy(r => r.RoomNumber)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new RoomListViewModel(rooms.Select(r => r.ToCardViewModel()).ToList(), page, pageSize, totalItems)
            {
                Filters = filters
            };
        }

        public async Task<DetailsRoomViewModel?> GetDetailsByIdAsync(int id)
        {
            var room = await _context.Rooms
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);

            return room?.ToDetailsViewModel();
        }

        public async Task<EditRoomViewModel?> GetForEditAsync(int id)
        {
            var room = await _context.Rooms
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);

            return room?.ToEditViewModel();
        }

        public async Task<DeleteRoomViewModel?> GetForDeleteAsync(int id)
        {
            var room = await _context.Rooms
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);

            if (room is null) return null;

            var activeReservationCount = await _context.Reservations
                .CountAsync(res => res.RoomId == id && res.CheckOutDate >= DateTime.Today);

            return room.ToDeleteViewModel(activeReservationCount);
        }

        /// <summary>
        /// Builds a pre-populated CreateRoomViewModel where RoomNumber defaults
        /// to one above the current highest room number (or 1 if no rooms exist yet).
        /// </summary>
        public async Task<CreateRoomViewModel> BuildCreateFormAsync()
        {
            var nextRoomNumber = await _context.Rooms.AnyAsync()
                ? await _context.Rooms.MaxAsync(r => r.RoomNumber) + 1
                : 1;

            return new CreateRoomViewModel
            {
                RoomNumber = nextRoomNumber
            };
        }

        /// <summary>
        /// Creates a new room. Returns a failure result if the room number is already taken.
        /// </summary>
        public async Task<ServiceResult> CreateAsync(CreateRoomViewModel model)
        {
            var duplicate = await _context.Rooms
                .AnyAsync(r => r.RoomNumber == model.RoomNumber);

            if (duplicate)
                return ServiceResult.Fail($"Room number {model.RoomNumber} is already in use.");

            _context.Rooms.Add(model.ToDomain());
            await _context.SaveChangesAsync();
            return ServiceResult.Ok();
        }

        /// <summary>
        /// Updates a room. Returns a failure result if the new room number conflicts
        /// with a different existing room.
        /// </summary>
        public async Task<ServiceResult> UpdateAsync(EditRoomViewModel model)
        {
            var room = await _context.Rooms.FindAsync(model.Id);
            if (room is null) return ServiceResult.Fail("Room not found.");

            if (room.RoomNumber != model.RoomNumber)
            {
                var duplicate = await _context.Rooms
                    .AnyAsync(r => r.RoomNumber == model.RoomNumber && r.Id != model.Id);

                if (duplicate)
                    return ServiceResult.Fail($"Room number {model.RoomNumber} is already in use.");
            }

            room.ApplyFromViewModel(model);
            await _context.SaveChangesAsync();
            return ServiceResult.Ok();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var room = await _context.Rooms
                .Include(r => r.Reservations)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (room is null) return false;
            if (room.Reservations.Any())
                throw new InvalidOperationException($"Room cannot be deleted because it has {room.Reservations.Count} associated reservation(s).");

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id) =>
            await _context.Rooms.AnyAsync(r => r.Id == id);
    }
}