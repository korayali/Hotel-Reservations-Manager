using HotelReservationsManager.Enums;
using HotelReservationsManager.Models.Domains;
using HotelReservationsManager.Models.ViewModels.User;

namespace HotelReservationsManager.Extensions.Mapping
{
    public static class UserMappingExtensions
    {
        public static DetailsUserViewModel ToDetailsViewModel(
          this User user,
          UserRole role)
        {
            return new DetailsUserViewModel
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                EGN = user.EGN,
                Email = user.Email ?? string.Empty,
                PhoneNumber = user.PhoneNumber,
                HireDate = user.HireDate,
                IsActive = user.IsActive,
                TerminationDate = user.TerminationDate,
                Role = role
            };
        }

        public static UserEditViewModel ToEditViewModel(
          this User user,
          UserRole role)
        {
            return new UserEditViewModel
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                EGN = user.EGN,
                Email = user.Email ?? string.Empty,
                PhoneNumber = user.PhoneNumber,
                HireDate = user.HireDate,
                IsActive = user.IsActive,
                TerminationDate = user.TerminationDate,
                Role = role
            };
        }
    }
}
