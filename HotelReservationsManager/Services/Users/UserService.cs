using HotelReservationsManager.Data;
using HotelReservationsManager.Enums;
using HotelReservationsManager.Models.Domains;
using HotelReservationsManager.Models.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HotelReservationsManager.Extensions.Mapping;
using System.Runtime.InteropServices;

namespace HotelReservationsManager.Services.Users
{
    public class UserService : IUserService
    {
        private readonly HotelReservationsManagerDbContext _context;
        private readonly UserManager<User> _userManager;

        public UserService(HotelReservationsManagerDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<UserListViewModel> GetPagedUsersAsync(UserFilterViewModel filters, int page, int pageSize)
        {
            var query = _context.Users.AsQueryable();

            // Text filters
            if (!string.IsNullOrWhiteSpace(filters.FirstName))
                query = query.Where(u => u.FirstName.Contains(filters.FirstName));

            if (!string.IsNullOrWhiteSpace(filters.LastName))
                query = query.Where(u => u.LastName.Contains(filters.LastName));

            if (!string.IsNullOrWhiteSpace(filters.Email))
                query = query.Where(u => u.Email != null && u.Email.Contains(filters.Email));

            // Status filter
            query = filters.StatusFilter switch
            {
                UserStatusFilter.Active => query.Where(u => u.IsActive),
                UserStatusFilter.Inactive => query.Where(u => !u.IsActive),
                _ => query
            };

            // Role filter
            if (filters.RoleFilter != UserRoleFilter.All)
            {
                var targetRoleName = filters.RoleFilter switch
                {
                    UserRoleFilter.AdminsOnly => UserRole.Admin.ToString(),
                    UserRoleFilter.EmployeesOnly => UserRole.Employee.ToString(),
                    _ => null
                };

                if (targetRoleName is not null)
                {
                    var roleId = await _context.Roles
                        .Where(r => r.Name == targetRoleName)
                        .Select(r => r.Id)
                        .FirstOrDefaultAsync();

                    if (roleId is not null)
                        query = query.Where(u => _context.UserRoles
                            .Any(ur => ur.UserId == u.Id && ur.RoleId == roleId));
                }
            }

            var totalItems = await query.CountAsync();

            var users = await query
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new
                {
                    u.Id,
                    u.DisplayName,
                    u.FirstName,
                    u.MiddleName,
                    u.LastName,
                    u.Email,
                    u.PhoneNumber,
                    u.HireDate,
                    u.IsActive,
                    ReservationCount = u.Reservations.Count
                })
                .ToListAsync();

            // Resolve Identity roles for each user in one query
            var userIds = users.Select(u => u.Id).ToList();
            var roleMap = await BuildRoleMapAsync(userIds);

            var cards = users.Select(u => new UserCardViewModel
            {
                Id = u.Id,
                DisplayName = u.DisplayName,
                FirstName = u.FirstName,
                MiddleName = u.MiddleName,
                LastName = u.LastName,
                Email = u.Email ?? string.Empty,
                PhoneNumber = u.PhoneNumber,
                HireDate = u.HireDate,
                IsActive = u.IsActive,
                Role = roleMap.TryGetValue(u.Id, out var role) ? role : UserRole.Employee,
                ReservationCount = u.ReservationCount
            }).ToList();

            return new UserListViewModel(cards, page, pageSize, totalItems)
            {
                Filters = filters
            };
        }

        /// <summary>Builds a userId → UserRole map for a batch of users in two queries.</summary>
        private async Task<Dictionary<string, UserRole>> BuildRoleMapAsync(List<string> userIds)
        {
            var adminRoleId = await _context.Roles
                .Where(r => r.Name == UserRole.Admin.ToString())
                .Select(r => r.Id)
                .FirstOrDefaultAsync();
 
            HashSet<string> adminIds = [];
 
            if (adminRoleId is not null)
            {
                adminIds = (await _context.UserRoles
                    .Where(ur => userIds.Contains(ur.UserId) && ur.RoleId == adminRoleId)
                    .Select(ur => ur.UserId)
                    .ToListAsync()).ToHashSet();
            }
 
            return userIds.ToDictionary(
                id => id,
                id => adminIds.Contains(id) ? UserRole.Admin : UserRole.Employee);
        }
    }
}
