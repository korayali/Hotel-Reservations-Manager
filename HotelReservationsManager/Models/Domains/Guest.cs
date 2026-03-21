using System.ComponentModel.DataAnnotations;

namespace HotelReservationsManager.Models.Domains
{
    public class Guest : BaseEntity
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool isAdult { get; set; } = true;
    }
}
