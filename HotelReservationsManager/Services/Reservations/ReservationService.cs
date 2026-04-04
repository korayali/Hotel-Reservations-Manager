using HotelReservationsManager.Data;
using HotelReservationsManager.Enums;
using HotelReservationsManager.Models.Domains;
using HotelReservationsManager.Models.ViewModels.Reservation;
using HotelReservationsManager.Models.ViewModels.ReservationGuest;
using Microsoft.EntityFrameworkCore;

namespace HotelReservationsManager.Services.Reservations
{
    public class ReservationService
    {
        private readonly HotelReservationsManagerDbContext _context;

        public ReservationService(HotelReservationsManagerDbContext context)
        {
            _context = context;
        }

        public async Task<ReservationListViewModel> GetReservationsAsync(ReservationFilterViewModel filters, int page, int pageSize)
        {
            var query = _context.Reservations
                .Include(r => r.Room)
                .Include(r => r.User)
                .Include(r => r.ReservationGuests)
                    .ThenInclude(rg => rg.Guest)
                .AsQueryable();

            // Room number
            if (!string.IsNullOrWhiteSpace(filters.RoomNumber))
                query = query.Where(r => r.Room.RoomNumber.ToString().Contains(filters.RoomNumber));

            // Booked by user name
            if (!string.IsNullOrWhiteSpace(filters.BookedByUserName))
                query = query.Where(r => r.User.UserName!.Contains(filters.BookedByUserName));

            // Check-in date range
            if (filters.CheckInFrom.HasValue)
                query = query.Where(r => r.CheckInDate >= filters.CheckInFrom.Value);

            if (filters.CheckInTo.HasValue)
                query = query.Where(r => r.CheckInDate <= filters.CheckInTo.Value);

            // Breakfast filter
            if (filters.BreakfastFilter == ReservationBoolFilter.Yes)
                query = query.Where(r => r.HasBreakfast);
            else if (filters.BreakfastFilter == ReservationBoolFilter.No)
                query = query.Where(r => !r.HasBreakfast);

            // All inclusive filter
            if (filters.AllInclusiveFilter == ReservationBoolFilter.Yes)
                query = query.Where(r => r.IsAllInclusive);
            else if (filters.AllInclusiveFilter == ReservationBoolFilter.No)
                query = query.Where(r => !r.IsAllInclusive);

            var totalCount = await query.CountAsync();

            var reservations = await query
                .OrderByDescending(r => r.CheckInDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var cards = reservations.Select(r => new ReservationCardViewModel
            {
                Id = r.Id,
                RoomNumber = r.Room.RoomNumber.ToString(),
                RoomTypeDisplay = r.Room.Type.ToString(),
                BookedByUserName = r.User.UserName!,
                CheckInDate = r.CheckInDate,
                CheckOutDate = r.CheckOutDate,
                GuestCount = r.ReservationGuests.Count,
                HasBreakfast = r.HasBreakfast,
                IsAllInclusive = r.IsAllInclusive,
                TotalPrice = r.TotalPrice
            });

            return new ReservationListViewModel(cards, page, pageSize, totalCount)
            {
                Filters = filters
            };
        }

        public async Task<DetailsReservationViewModel?> GetReservationDetailsAsync(int id)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Room)
                .Include(r => r.User)
                .Include(r => r.ReservationGuests)
                    .ThenInclude(rg => rg.Guest)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null)
                return null;

            return new DetailsReservationViewModel
            {
                Id = reservation.Id,
                RoomNumber = reservation.Room.RoomNumber.ToString(),
                RoomTypeDisplay = reservation.Room.Type.ToString(),
                RoomCapacity = reservation.Room.Capacity,
                BookedByUserName = reservation.User.UserName!,
                BookedByUserId = reservation.UserId,
                CheckInDate = reservation.CheckInDate,
                CheckOutDate = reservation.CheckOutDate,
                HasBreakfast = reservation.HasBreakfast,
                IsAllInclusive = reservation.IsAllInclusive,
                TotalPrice = reservation.TotalPrice,
                Guests = reservation.ReservationGuests.Select(rg => new ReservationGuestCardViewModel
                {
                    GuestId = rg.GuestId,
                    FullName = $"{rg.Guest.FirstName} {rg.Guest.LastName}",
                    Email = rg.Guest.Email,
                    PhoneNumber = rg.Guest.PhoneNumber,
                    IsAdult = rg.Guest.isAdult
                })
            };
        }

        public async Task<int> CreateReservationAsync(CreateReservationViewModel vm)
        {
            var reservation = new Reservation
            {
                RoomId = vm.RoomId,
                UserId = vm.UserId,
                CheckInDate = vm.CheckInDate,
                CheckOutDate = vm.CheckOutDate,
                HasBreakfast = vm.HasBreakfast,
                IsAllInclusive = vm.IsAllInclusive
            };

            reservation.ReservationGuests = vm.GuestIds
                .Distinct()
                .Select(id => new ReservationGuest
                {
                    GuestId = id
                })
                .ToList();

            reservation.TotalPrice = await CalculateTotalPriceAsync(reservation);

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            return reservation.Id;
        }

        public async Task<EditReservationViewModel?> GetEditReservationAsync(int id)
        {
            var reservation = await _context.Reservations
                .Include(r => r.ReservationGuests)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null)
                return null;

            return new EditReservationViewModel
            {
                Id = reservation.Id,
                RoomId = reservation.RoomId,
                UserId = reservation.UserId,
                CheckInDate = reservation.CheckInDate,
                CheckOutDate = reservation.CheckOutDate,
                HasBreakfast = reservation.HasBreakfast,
                IsAllInclusive = reservation.IsAllInclusive,
                TotalPrice = reservation.TotalPrice,
                GuestIds = reservation.ReservationGuests.Select(rg => rg.GuestId).ToList()
            };
        }

        public async Task UpdateReservationAsync(EditReservationViewModel vm)
        {
            var reservation = await _context.Reservations
               .Include(r => r.ReservationGuests)
               .FirstOrDefaultAsync(r => r.Id == vm.Id)
               ?? throw new InvalidOperationException($"Reservation '{vm.Id}' not found.");

            reservation.RoomId = vm.RoomId;
            reservation.UserId = vm.UserId;
            reservation.CheckInDate = vm.CheckInDate;
            reservation.CheckOutDate = vm.CheckOutDate;
            reservation.HasBreakfast = vm.HasBreakfast;
            reservation.IsAllInclusive = vm.IsAllInclusive;

            reservation.ReservationGuests.Clear();
            foreach (var guestId in vm.GuestIds.Distinct())
            {
                reservation.ReservationGuests.Add(new ReservationGuest
                {
                    ReservationId = reservation.Id,
                    GuestId = guestId
                });
            }

            reservation.TotalPrice = await CalculateTotalPriceAsync(reservation);

            await _context.SaveChangesAsync();
        }

        public async Task CancelReservationAsync(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id)
                ?? throw new InvalidOperationException($"Reservation '{id}' not found.");

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
        }

        private async Task<decimal> CalculateTotalPriceAsync(Reservation reservation)
        {
            var room = await _context.Rooms.FirstAsync(r => r.Id == reservation.RoomId);

            var nights = (reservation.CheckOutDate - reservation.CheckInDate).Days;
            if (nights <= 0) return 0m;

            var guestCount = reservation.ReservationGuests.Count;

            var basePrice = (decimal)room.PricePerAdult * guestCount * nights;

            if (reservation.HasBreakfast)
                basePrice += basePrice * 0.1m;

            if (reservation.IsAllInclusive)
                basePrice += basePrice * 0.3m;

            return basePrice;
        }
    }
}

