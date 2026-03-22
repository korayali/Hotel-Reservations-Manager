using System.ComponentModel.DataAnnotations;

namespace HotelReservationsManager.Models.ViewModels.User
{
    public class UserFilterViewModel
    {
        [Display(Name = "First Name")]
        [StringLength(50)]
        public string? FirstName { get; set; }

        [Display(Name = "Last Name")]
        [StringLength(50)]
        public string? LastName { get; set; }

        [Display(Name = "Email")]
        [StringLength(100)]
        [EmailAddress]
        public string? Email { get; set; }

        [Display(Name = "Status")]
        public UserStatusFilter StatusFilter { get; set; } = UserStatusFilter.All;

        [Display(Name = "Role")]
        public UserRoleFilter RoleFilter { get; set; } = UserRoleFilter.All;

        public bool HasActiveFilters() =>
            !string.IsNullOrWhiteSpace(FirstName) ||
            !string.IsNullOrWhiteSpace(LastName) ||
            !string.IsNullOrWhiteSpace(Email) ||
            StatusFilter != UserStatusFilter.All ||
            RoleFilter != UserRoleFilter.All;
    }

    public enum UserStatusFilter
    {
        All,
        Active,
        Inactive
    }

    public enum UserRoleFilter
    {
        All,
        AdminsOnly,
        EmployeesOnly
    }
}
