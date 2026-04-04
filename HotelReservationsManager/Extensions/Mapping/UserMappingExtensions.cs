using HotelReservationsManager.Enums;
using HotelReservationsManager.Models.Domains;
using HotelReservationsManager.Models.ViewModels.User;
using Microsoft.AspNetCore.Identity;

namespace HotelReservationsManager.Extensions.Mapping
{
    public static class UserMappingExtensions
    {
        public static async Task<UserCardViewModel> ToCardViewModelAsync(this User user, UserManager<User> userManager)
        {
            var roles = await userManager.GetRolesAsync(user);
            return new UserCardViewModel
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                Email = user.Email!,
                PhoneNumber = user.PhoneNumber,
                Role = roles.Contains("Admin") ? UserRole.Admin : UserRole.Employee,
                HireDate = user.HireDate,
                IsActive = user.IsActive,
                ReservationCount = user.Reservations.Count
            };
        }

        public static async Task<DetailsUserViewModel> ToDetailsViewModelAsync(this User user, UserManager<User> userManager)
        {
            var roles = await userManager.GetRolesAsync(user);
            return new DetailsUserViewModel
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                EGN = user.EGN,
                Email = user.Email!,
                PhoneNumber = user.PhoneNumber,
                Role = roles.Contains("Admin") ? UserRole.Admin : UserRole.Employee,
                HireDate = user.HireDate,
                IsActive = user.IsActive,
                TerminationDate = user.TerminationDate
            };
        }

        public static void ApplyFromViewModel(this User user, DetailsUserViewModel vm)
        {
            user.DisplayName = vm.DisplayName;
            user.FirstName = vm.FirstName;
            user.MiddleName = vm.MiddleName;
            user.LastName = vm.LastName;
            user.EGN = vm.EGN;
            user.Email = vm.Email;
            user.PhoneNumber = vm.PhoneNumber;
            user.HireDate = vm.HireDate;
            user.IsActive = vm.IsActive;
            user.TerminationDate = vm.IsActive ? null : vm.TerminationDate;
        }
    }
}
