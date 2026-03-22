using HotelReservationsManager.Enums;
using System.ComponentModel.DataAnnotations;

namespace HotelReservationsManager.Models.ViewModels.Room
{
    public class RoomFilterViewModel
    {
        [Display(Name = "Room type")]
        public RoomType? Type { get; set; }

        [Display(Name = "Availability")]
        public RoomAvailabilityFilter AvailabilityFilter { get; set; } = RoomAvailabilityFilter.All;

        [Display(Name = "Min capacity")]
        [Range(1, 20)]
        public int? MinCapacity { get; set; }

        [Display(Name = "Max capacity")]
        [Range(1, 20)]
        public int? MaxCapacity { get; set; }

        public bool HasActiveFilters() =>
            Type.HasValue ||
            AvailabilityFilter != RoomAvailabilityFilter.All ||
            MinCapacity.HasValue ||
            MaxCapacity.HasValue;
    }

    public enum RoomAvailabilityFilter
    {
        All,
        AvailableOnly,
        OccupiedOnly
    }
}
