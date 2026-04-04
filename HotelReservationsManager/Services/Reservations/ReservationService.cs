using HotelReservationsManager.Data;
using HotelReservationsManager.Extensions.Mapping;
using HotelReservationsManager.Models.Domains;
using HotelReservationsManager.Models.ViewModels.Reservation;
using HotelReservationsManager.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HotelReservationsManager.Services
{
    public class ReservationService : IReservationService
    {
        private readonly HotelReservationsManagerDbContext _context;

        public ReservationService(HotelReservationsManagerDbContext context)
        {
            _context = context;
        }

        public async Task<ReservationListViewModel> GetAllAsync(ReservationFilterViewModel filters, int page, int pageSize)
        {
            var query = _context.Reservations
                .AsNoTracking()
                .Include(r => r.Room)
                .Include(r => r.User)
                .Include(r => r.ReservationGuests)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filters.RoomNumber))
                query = query.Where(r => r.Room.RoomNumber.ToString().Contains(filters.RoomNumber));

            if (!string.IsNullOrWhiteSpace(filters.BookedByUserName))
                query = query.Where(r => r.User.DisplayName.Contains(filters.BookedByUserName));

            if (filters.CheckInFrom.HasValue)
                query = query.Where(r => r.CheckInDate >= filters.CheckInFrom.Value);

            if (filters.CheckInTo.HasValue)
                query = query.Where(r => r.CheckInDate <= filters.CheckInTo.Value);

            if (filters.BreakfastFilter == ReservationBoolFilter.Yes)
                query = query.Where(r => r.HasBreakfast);
            else if (filters.BreakfastFilter == ReservationBoolFilter.No)
                query = query.Where(r => !r.HasBreakfast);

            if (filters.AllInclusiveFilter == ReservationBoolFilter.Yes)
                query = query.Where(r => r.IsAllInclusive);
            else if (filters.AllInclusiveFilter == ReservationBoolFilter.No)
                query = query.Where(r => !r.IsAllInclusive);

            int totalItems = await query.CountAsync();

            var reservations = await query
                .OrderByDescending(r => r.CheckInDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var cards = reservations.Select(r => r.ToCardViewModel()).ToList();

            if (filters.StatusFilter != ReservationStatusFilter.All)
            {
                var statusName = filters.StatusFilter.ToString();
                cards = cards.Where(c => c.Status.ToString() == statusName).ToList();
            }

            return new ReservationListViewModel(cards, page, pageSize, totalItems)
            {
                Filters = filters
            };
        }

        public async Task<DetailsReservationViewModel?> GetDetailsByIdAsync(int id)
        {
            var reservation = await _context.Reservations
                .AsNoTracking()
                .Include(r => r.Room)
                .Include(r => r.User)
                .Include(r => r.ReservationGuests)
                    .ThenInclude(rg => rg.Guest)
                .FirstOrDefaultAsync(r => r.Id == id);

            return reservation?.ToDetailsViewModel();
        }

        public async Task<EditReservationViewModel?> GetForEditAsync(int id)
        {
            var reservation = await _context.Reservations
                .AsNoTracking()
                .Include(r => r.ReservationGuests)
                .FirstOrDefaultAsync(r => r.Id == id);

            return reservation?.ToEditViewModel();
        }

        public async Task<DeleteReservationViewModel?> GetForDeleteAsync(int id)
        {
            var reservation = await _context.Reservations
                .AsNoTracking()
                .Include(r => r.Room)
                .Include(r => r.User)
                .Include(r => r.ReservationGuests)
                .FirstOrDefaultAsync(r => r.Id == id);

            return reservation?.ToDeleteViewModel();
        }

        public async Task<CreateReservationViewModel> BuildCreateFormAsync()
        {
            return new CreateReservationViewModel();
        }

        public async Task<EditReservationViewModel?> BuildEditFormAsync(int id)
        {
            var vm = await GetForEditAsync(id);
            if (vm is null) return null;
            await PopulateDropdownsAsync(vm);
            return vm;
        }

        public async Task CreateAsync(CreateReservationViewModel model, string userId)
        {
            var reservation = model.ToDomain();
            reservation.UserId = userId;
            reservation.TotalPrice = await CalculatePriceAsync(
                model.RoomId,
                model.GuestIds,
                model.CheckInDate,
                model.CheckOutDate,
                model.HasBreakfast,
                model.IsAllInclusive);

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            await SyncGuestsAsync(reservation.Id, model.GuestIds);
            await RefreshRoomAvailabilityAsync(model.RoomId);
        }

        public async Task<bool> UpdateAsync(EditReservationViewModel model, string userId)
        {
            var reservation = await _context.Reservations
                .Include(r => r.ReservationGuests)
                .FirstOrDefaultAsync(r => r.Id == model.Id);

            if (reservation is null) return false;

            var previousRoomId = reservation.RoomId;

            reservation.ApplyFromViewModel(model);
            reservation.TotalPrice = await CalculatePriceAsync(
                model.RoomId, model.GuestIds, model.CheckInDate, model.CheckOutDate,
                model.HasBreakfast, model.IsAllInclusive);
            reservation.UserId = userId;

            await _context.SaveChangesAsync();
            await SyncGuestsAsync(reservation.Id, model.GuestIds);

            await RefreshRoomAvailabilityAsync(model.RoomId);
            if (model.RoomId != previousRoomId)
                await RefreshRoomAvailabilityAsync(previousRoomId);

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation is null) return false;

            var roomId = reservation.RoomId;

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();

            await RefreshRoomAvailabilityAsync(roomId);
            return true;
        }

        /// <summary>
        /// Soft capacity check. Returns a human-readable warning string when
        /// guestCount exceeds the room's capacity, or null when within limits.
        /// The caller decides whether to block or just surface the warning.
        /// </summary>
        public async Task<string?> CheckCapacityAsync(int roomId, int guestCount)
        {
            var room = await _context.Rooms
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == roomId);

            if (room is null) return null;
            if (guestCount <= room.Capacity) return null;

            return $"Warning: Room {room.RoomNumber} has a capacity of {room.Capacity} " +
                   $"but {guestCount} guest(s) were selected. " +
                   $"The reservation was saved, but please verify this is intentional.";
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        /// <summary>
        /// Recomputes IsFree for a single room: the room is free when no reservation
        /// covers today (CheckInDate &lt;= today &lt; CheckOutDate).
        /// </summary>
        private async Task RefreshRoomAvailabilityAsync(int roomId)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room is null) return;

            var today = DateTime.Today;

            var isOccupied = await _context.Reservations
                .AnyAsync(r =>
                    r.RoomId == roomId &&
                    r.CheckInDate <= today &&
                    r.CheckOutDate > today);

            room.IsFree = !isOccupied;
            await _context.SaveChangesAsync();
        }

        private async Task SyncGuestsAsync(int reservationId, List<int> guestIds)
        {
            var existing = await _context.ReservationGuests
                .Where(rg => rg.ReservationId == reservationId)
                .ToListAsync();

            _context.ReservationGuests.RemoveRange(existing);

            _context.ReservationGuests.AddRange(guestIds.Select(gid => new ReservationGuest
            {
                ReservationId = reservationId,
                GuestId = gid
            }));

            await _context.SaveChangesAsync();
        }

        private async Task<decimal> CalculatePriceAsync(
            int roomId, List<int> guestIds,
            DateTime checkIn, DateTime checkOut,
            bool hasBreakfast, bool isAllInclusive)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room is null) return 0;

            int nights = Math.Max(1, (checkOut - checkIn).Days);

            var guests = await _context.Guests
                .Where(g => guestIds.Contains(g.Id))
                .ToListAsync();

            int adults = guests.Count(g => g.isAdult);
            int children = guests.Count(g => !g.isAdult);

            decimal basePrice = (decimal)(adults * room.PricePerAdult + children * room.PricePerChild) * nights;

            if (hasBreakfast) basePrice *= 1.05m;
            if (isAllInclusive) basePrice *= 1.20m;

            return Math.Round(basePrice, 2);
        }

        private async Task PopulateDropdownsAsync(dynamic vm)
        {
            vm.AvailableRooms = await _context.Rooms
                .Where(r => r.IsFree)
                .OrderBy(r => r.RoomNumber)
                .Select(r => new SelectListItem(
                    $"Room {r.RoomNumber} — {r.Type} (cap. {r.Capacity})",
                    r.Id.ToString()))
                .ToListAsync();

            vm.AvailableGuests = await _context.Guests
                .OrderBy(g => g.LastName).ThenBy(g => g.FirstName)
                .Select(g => new SelectListItem(
                    $"{g.FirstName} {g.LastName} ({g.Email})",
                    g.Id.ToString()))
                .ToListAsync();
        }

        public async Task<List<Room>> GetAvailableRoomsAsync(DateTime checkIn, DateTime checkOut)
        {
            return await _context.Rooms
                .Where(room =>
                    !_context.Reservations.Any(r =>
                        r.RoomId == room.Id &&
                        r.CheckInDate < checkOut &&
                        r.CheckOutDate > checkIn
                    ))
                .OrderBy(r => r.RoomNumber)
                .ToListAsync();
        }

        public async Task<List<Guest>> GetAvailableGuestsAsync(DateTime checkIn, DateTime checkOut)
        {
            return await _context.Guests
                .Where(g =>
                    !_context.ReservationGuests.Any(rg =>
                        rg.GuestId == g.Id &&
                        rg.Reservation.CheckInDate < checkOut &&
                        rg.Reservation.CheckOutDate > checkIn
                    ))
                .OrderBy(g => g.LastName)
                .ThenBy(g => g.FirstName)
                .ToListAsync();
        }
    }
}