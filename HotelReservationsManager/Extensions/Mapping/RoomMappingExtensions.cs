using HotelReservationsManager.Models.Domains;
using HotelReservationsManager.Models.ViewModels.Room;

namespace HotelReservationsManager.Extensions.Mapping
{
    public static class RoomMappingExtensions
    {
        public static RoomCardViewModel ToCardViewModel(this Room room) =>
            new RoomCardViewModel
            {
                Id             = room.Id,
                RoomNumber     = room.RoomNumber,
                Type           = room.Type,
                Capacity       = room.Capacity,
                IsFree         = room.IsFree,
                PricePerAdult  = (decimal)room.PricePerAdult,
                PricePerChild  = (decimal)room.PricePerChild
            };

        public static DetailsRoomViewModel ToDetailsViewModel(this Room room) =>
            new DetailsRoomViewModel
            {
                Id             = room.Id,
                RoomNumber     = room.RoomNumber,
                Type           = room.Type,
                Capacity       = room.Capacity,
                IsFree         = room.IsFree,
                PricePerAdult  = (decimal)room.PricePerAdult,
                PricePerChild  = (decimal)room.PricePerChild
            };

        public static EditRoomViewModel ToEditViewModel(this Room room) =>
            new EditRoomViewModel
            {
                Id             = room.Id,
                RoomNumber     = room.RoomNumber,
                Type           = room.Type,
                Capacity       = room.Capacity,
                IsFree         = room.IsFree,
                PricePerAdult  = (decimal)room.PricePerAdult,
                PricePerChild  = (decimal)room.PricePerChild
            };

        public static DeleteRoomViewModel ToDeleteViewModel(this Room room, int activeReservationCount) =>
            new DeleteRoomViewModel
            {
                Id                   = room.Id,
                RoomNumber           = room.RoomNumber,
                TypeDisplay          = room.Type.ToString(),
                Capacity             = room.Capacity,
                ActiveReservationCount = activeReservationCount
            };

        public static Room ToDomain(this CreateRoomViewModel vm) =>
            new Room
            {
                RoomNumber    = vm.RoomNumber,
                Type          = vm.Type,
                Capacity      = vm.Capacity,
                IsFree        = vm.IsFree,
                PricePerAdult = (double)vm.PricePerAdult,
                PricePerChild = (double)vm.PricePerChild
            };

        public static void ApplyFromViewModel(this Room room, EditRoomViewModel vm)
        {
            room.RoomNumber    = vm.RoomNumber;
            room.Type          = vm.Type;
            room.Capacity      = vm.Capacity;
            room.IsFree        = vm.IsFree;
            room.PricePerAdult = (double)vm.PricePerAdult;
            room.PricePerChild = (double)vm.PricePerChild;
        }
    }
}
