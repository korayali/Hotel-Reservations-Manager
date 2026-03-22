using System.ComponentModel.DataAnnotations;

namespace HotelReservationsManager.Models.ViewModels.Guest
{
    public class CreateGuestViewModel
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = null!;

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        public bool IsAdult { get; set; } = true;
    }
}