using HotelReservationsManager.Data;
using HotelReservationsManager.Extensions.Mapping;
using HotelReservationsManager.Models;
using HotelReservationsManager.Models.ViewModels.Guest;
using HotelReservationsManager.Models.ViewModels.ReservationGuest;
using HotelReservationsManager.Models.ViewModels.Shared;
using HotelReservationsManager.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelReservationsManager.Services.Guests
{
    public class GuestService : IGuestService
    {
        private readonly HotelReservationsManagerDbContext _context;

        public GuestService(HotelReservationsManagerDbContext context)
        {
            _context = context;
        }

        public async Task<GuestListViewModel> GetAllAsync(GuestFilterViewModel filters, int page, int pageSize)
        {
            var query = _context.Guests.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(filters.FirstName))
                query = query.Where(g => g.FirstName.Contains(filters.FirstName));

            if (!string.IsNullOrWhiteSpace(filters.LastName))
                query = query.Where(g => g.LastName.Contains(filters.LastName));

            if (!string.IsNullOrWhiteSpace(filters.Email))
                query = query.Where(g => g.Email.Contains(filters.Email));

            query = filters.AgeFilter switch
            {
                GuestAgeFilter.AdultsOnly => query.Where(g => g.isAdult),
                GuestAgeFilter.MinorsOnly => query.Where(g => !g.isAdult),
                _ => query
            };

            query = filters.ReservationFilter switch
            {
                GuestReservationFilter.WithReservations => query.Where(g => g.ReservationGuests.Any()),
                GuestReservationFilter.WithoutReservations => query.Where(g => !g.ReservationGuests.Any()),
                _ => query
            };

            int totalItems = await query.CountAsync();

            var guests = await query
                .OrderBy(g => g.LastName)
                .ThenBy(g => g.FirstName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var items = guests.Select(g => g.ToCardViewModel()).ToList();

            return new GuestListViewModel(items, page, pageSize, totalItems)
            {
                Filters = filters
            };
        }

        public async Task<DetailsGuestViewModel?> GetDetailsByIdAsync(int id, int reservationsPage, int reservationsPageSize)
        {
            var guest = await _context.Guests
                .AsNoTracking()
                .Include(g => g.ReservationGuests)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (guest is null) return null;

            var vm = guest.ToDetailsViewModel();

            var reservationsQuery = _context.ReservationGuests
                .AsNoTracking()
                .Where(rg => rg.GuestId == id)
                .Include(rg => rg.Reservation).ThenInclude(r => r.Room)
                .Include(rg => rg.Reservation).ThenInclude(r => r.User)
                .Include(rg => rg.Reservation);

            int totalReservations = await reservationsQuery.CountAsync();

            var reservations = await reservationsQuery
                .OrderByDescending(rg => rg.Reservation.CheckInDate)
                .Skip((reservationsPage - 1) * reservationsPageSize)
                .Take(reservationsPageSize)
                .ToListAsync();

            vm.Reservations = new PageResultViewModel<GuestReservationCardViewModel>(
                reservations.Select(rg => rg.ToCardViewModel()).ToList(),
                reservationsPage,
                reservationsPageSize,
                totalReservations);

            return vm;
        }

        public async Task<EditGuestViewModel?> GetForEditAsync(int id)
        {
            var guest = await _context.Guests
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == id);

            return guest?.ToEditViewModel();
        }

        public async Task<DeleteGuestViewModel?> GetForDeleteAsync(int id)
        {
            var guest = await _context.Guests
                .AsNoTracking()
                .Include(g => g.ReservationGuests)
                .FirstOrDefaultAsync(g => g.Id == id);

            return guest?.ToDeleteViewModel();
        }

        /// <summary>
        /// Creates a new guest. Returns a failure result if the email or phone
        /// number is already registered to another guest.
        /// </summary>
        public async Task<ServiceResult> CreateAsync(CreateGuestViewModel model)
        {
            var emailTaken = await _context.Guests
                .AnyAsync(g => g.Email == model.Email);

            if (emailTaken)
                return ServiceResult.Fail($"A guest with email '{model.Email}' already exists.");

            var phoneTaken = await _context.Guests
                .AnyAsync(g => g.PhoneNumber == model.PhoneNumber);

            if (phoneTaken)
                return ServiceResult.Fail($"A guest with phone number '{model.PhoneNumber}' already exists.");

            var guest = model.ToDomain();
            _context.Guests.Add(guest);
            await _context.SaveChangesAsync();
            return ServiceResult.Ok();
        }

        /// <summary>
        /// Updates a guest. Returns a failure result if the new email or phone
        /// number conflicts with a different existing guest.
        /// </summary>
        public async Task<ServiceResult> UpdateAsync(EditGuestViewModel model)
        {
            var guest = await _context.Guests.FindAsync(model.Id);
            if (guest is null) return ServiceResult.Fail("Guest not found.");

            // Only check uniqueness for fields that actually changed
            if (!string.Equals(guest.Email, model.Email, StringComparison.OrdinalIgnoreCase))
            {
                var emailTaken = await _context.Guests
                    .AnyAsync(g => g.Email == model.Email && g.Id != model.Id);

                if (emailTaken)
                    return ServiceResult.Fail($"A guest with email '{model.Email}' already exists.");
            }

            if (guest.PhoneNumber != model.PhoneNumber)
            {
                var phoneTaken = await _context.Guests
                    .AnyAsync(g => g.PhoneNumber == model.PhoneNumber && g.Id != model.Id);

                if (phoneTaken)
                    return ServiceResult.Fail($"A guest with phone number '{model.PhoneNumber}' already exists.");
            }

            guest.ApplyFromViewModel(model);
            await _context.SaveChangesAsync();
            return ServiceResult.Ok();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var guest = await _context.Guests.FindAsync(id);
            if (guest is null) return false;

            _context.Guests.Remove(guest);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id) =>
            await _context.Guests.AnyAsync(g => g.Id == id);
    }
}