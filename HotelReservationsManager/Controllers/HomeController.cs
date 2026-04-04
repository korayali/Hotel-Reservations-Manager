using HotelReservationsManager.Data;
using HotelReservationsManager.Models;
using HotelReservationsManager.Models.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace HotelReservationsManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly HotelReservationsManagerDbContext _context;

        public HomeController(HotelReservationsManagerDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (!User.Identity!.IsAuthenticated)
                return View(null as HomeIndexViewModel);

            var today = DateOnly.FromDateTime(DateTime.Today);

            var vm = new HomeIndexViewModel
            {
                RoomsAvailable = await _context.Rooms.CountAsync(r => r.IsFree),
                ActiveReservations = await _context.Reservations.CountAsync(r =>
                                          r.CheckInDate.Date <= DateTime.Today &&
                                          r.CheckOutDate.Date >= DateTime.Today),
                RegisteredGuests = await _context.Guests.CountAsync(),
                CheckInsToday = await _context.Reservations.CountAsync(r =>
                                          r.CheckInDate.Date == DateTime.Today)
            };

            return View(vm);
        }

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() =>
            View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}