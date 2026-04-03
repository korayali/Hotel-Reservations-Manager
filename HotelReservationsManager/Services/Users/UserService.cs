using HotelReservationsManager.Data;
using HotelReservationsManager.Enums;
using HotelReservationsManager.Extensions.Mapping;
using HotelReservationsManager.Models.Domains;
using HotelReservationsManager.Models.ViewModels.User;
using HotelReservationsManager.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace HotelReservationsManager.Services.Users
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;

        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<UserListViewModel> GetUsersAsync(UserFilterViewModel filters, int page, int pageSize)
        {
            var query = _userManager.Users
                .Include(u => u.Reservations)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filters.FirstName))
                query = query.Where(u => u.FirstName.Contains(filters.FirstName));
            if (!string.IsNullOrWhiteSpace(filters.LastName))
                query = query.Where(u => u.LastName.Contains(filters.LastName));
            if (!string.IsNullOrWhiteSpace(filters.Email))
                query = query.Where(u => u.Email!.Contains(filters.Email));
            if (filters.StatusFilter == UserStatusFilter.Active)
                query = query.Where(u => u.IsActive);
            else if (filters.StatusFilter == UserStatusFilter.Inactive)
                query = query.Where(u => !u.IsActive);

            var users = await query.ToListAsync();

            var cards = new List<UserCardViewModel>();
            foreach (var user in users)
            {
                var card = await user.ToCardViewModelAsync(_userManager);

                if (filters.RoleFilter == UserRoleFilter.AdminsOnly && card.Role != UserRole.Admin) continue;
                if (filters.RoleFilter == UserRoleFilter.EmployeesOnly && card.Role != UserRole.Employee) continue;

                cards.Add(card);
            }

            var paged = cards
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new UserListViewModel(paged, page, pageSize, cards.Count)
            {
                Filters = filters
            };
        }

        public async Task<DetailsUserViewModel?> GetUserDetailsAsync(string id)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Id == id);

            return user == null ? null : await user.ToDetailsViewModelAsync(_userManager);
        }

        public async Task UpdateUserAsync(DetailsUserViewModel vm, string currentUserId)
        {
            var user = await _userManager.FindByIdAsync(vm.Id)
                ?? throw new InvalidOperationException($"User '{vm.Id}' not found.");

            var currentRoles = await _userManager.GetRolesAsync(user);
            var wasAdmin = currentRoles.Contains("Admin");
            var willBeAdmin = vm.Role == UserRole.Admin;

            if (wasAdmin && !willBeAdmin)
            {
                if (vm.Id == currentUserId)
                    throw new InvalidOperationException("You cannot remove your own admin role.");

                var adminUsers = await _userManager.GetUsersInRoleAsync("Admin");
                if (adminUsers.Count == 1)
                    throw new InvalidOperationException("Cannot remove the last administrator.");

                var originalAdmin = adminUsers.OrderBy(u => u.HireDate).First();
                if (originalAdmin.Id == vm.Id)
                    throw new InvalidOperationException("Cannot remove the original administrator.");
            }

            user.ApplyFromViewModel(vm);
            await _userManager.UpdateAsync(user);

            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            await _userManager.AddToRoleAsync(user, vm.Role.ToString());
        }

        public async Task ToggleAdminAsync(string id, string currentUserId)
        {
            var user = await _userManager.FindByIdAsync(id)
                ?? throw new InvalidOperationException($"User '{id}' not found.");

            if (id == currentUserId)
                throw new InvalidOperationException("You cannot remove your own admin role.");

            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains("Admin"))
            {
                var adminUsers = await _userManager.GetUsersInRoleAsync("Admin");
                if (adminUsers.Count == 1)
                    throw new InvalidOperationException("Cannot remove the last administrator.");

                // Original admin is the earliest hired admin
                var originalAdmin = adminUsers
                    .OrderBy(u => u.HireDate)
                    .First();

                if (originalAdmin.Id == id)
                    throw new InvalidOperationException("Cannot remove the original administrator.");

                await _userManager.RemoveFromRoleAsync(user, "Admin");
                await _userManager.AddToRoleAsync(user, "Employee");
            }
            else
            {
                await _userManager.RemoveFromRoleAsync(user, "Employee");
                await _userManager.AddToRoleAsync(user, "Admin");
            }
        }
    }
}
