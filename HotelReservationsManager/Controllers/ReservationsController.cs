using HotelReservationsManager.Models.ViewModels.Reservation;
using HotelReservationsManager.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HotelReservationsManager.Controllers
{
    [Authorize]
    public class ReservationsController : Controller
    {
        private readonly IReservationService _reservationService;
        private const int DefaultPageSize = 10;

        public ReservationsController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        // GET: /Reservations
        public async Task<IActionResult> Index(
            ReservationFilterViewModel filters,
            int page = 1,
            int pageSize = DefaultPageSize)
        {
            if (page < 1) page = 1;

            pageSize = pageSize switch
            {
                10 => 10,
                25 => 25,
                50 => 50,
                _ => DefaultPageSize
            };

            var model = await _reservationService.GetAllAsync(filters, page, pageSize);
            return View(model);
        }

        // GET: /Reservations/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var model = await _reservationService.GetDetailsByIdAsync(id);
            if (model is null) return NotFound();
            return View(model);
        }

        // GET: /Reservations/Create
        public async Task<IActionResult> Create()
        {
            var model = await _reservationService.BuildCreateFormAsync();
            return View(model);
        }

        // POST: /Reservations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateReservationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var rebuiltModel = await _reservationService.BuildCreateFormAsync();

                model.AvailableRooms = rebuiltModel.AvailableRooms;
                model.AvailableGuests = rebuiltModel.AvailableGuests;

                return View(model);
            }

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(currentUserId))
            {
                return Unauthorized();
            }

            await _reservationService.CreateAsync(model, currentUserId);

            TempData["Success"] = "Reservation created successfully.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Reservations/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _reservationService.BuildEditFormAsync(id);
            if (model is null) return NotFound();
            return View(model);
        }

        // POST: /Reservations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditReservationViewModel model)
        {
            if (id != model.Id) return BadRequest();

            if (!ModelState.IsValid)
            {
                await _reservationService.BuildEditFormAsync(id);
                return View(model);
            }

            var updated = await _reservationService.UpdateAsync(model);
            if (!updated) return NotFound();

            TempData["Success"] = "Reservation updated successfully.";
            return RedirectToAction(nameof(Details), new { id });
        }

        // GET: /Reservations/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var model = await _reservationService.GetForDeleteAsync(id);
            if (model is null) return NotFound();
            return View(model);
        }

        // POST: /Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deleted = await _reservationService.DeleteAsync(id);
            if (!deleted) return NotFound();

            TempData["Success"] = "Reservation deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> GetAvailableRooms(DateTime checkIn, DateTime checkOut)
        {
            var rooms = await _reservationService.GetAvailableRoomsAsync(checkIn, checkOut);

            return Json(rooms.Select(r => new
            {
                id = r.Id,
                text = $"Room {r.RoomNumber} — {r.Type}"
            }));
        }

        [HttpGet]
        public async Task<IActionResult> GetAvailableGuests(DateTime checkIn, DateTime checkOut)
        {
            var guests = await _reservationService.GetAvailableGuestsAsync(checkIn, checkOut);

            return Json(guests.Select(g => new
            {
                id = g.Id,
                text = $"{g.FirstName} {g.LastName} ({g.Email})"
            }));
        }
    }
}