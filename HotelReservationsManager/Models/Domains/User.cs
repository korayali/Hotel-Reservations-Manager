using Microsoft.AspNetCore.Identity;

namespace HotelReservationsManager.Models.Domains
{
    public class User : IdentityUser
    {
        public string DisplayName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string MiddleName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string EGN { get; set; } = null!;
        public DateOnly HireDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
        public bool IsActive { get; set; } = true;
        public DateOnly? TerminationDate { get; set; } = null;

    }
}
