using HotelReservationsManager.Models.ViewModels.Room;
using HotelReservationsManager.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservationsManager.Controllers
{
    [Authorize]
    public class RoomsController : Controller
    {
        private readonly IRoomService _roomService;
        private const int DefaultPageSize = 10;

        public RoomsController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        // GET: /Rooms
        public async Task<IActionResult> Index(
            RoomFilterViewModel filters,
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

            var model = await _roomService.GetAllAsync(filters, page, pageSize);
            return View(model);
        }

        // GET: /Rooms/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var model = await _roomService.GetDetailsByIdAsync(id);
            if (model is null) return NotFound();
            return View(model);
        }

        // GET: /Rooms/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create() => View(new CreateRoomViewModel());

        // POST: /Rooms/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateRoomViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            await _roomService.CreateAsync(model);
            TempData["Success"] = $"Room {model.RoomNumber} created successfully.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Rooms/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _roomService.GetForEditAsync(id);
            if (model is null) return NotFound();
            return View(model);
        }

        // POST: /Rooms/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, EditRoomViewModel model)
        {
            if (id != model.Id) return BadRequest();
            if (!ModelState.IsValid) return View(model);

            var updated = await _roomService.UpdateAsync(model);
            if (!updated) return NotFound();

            TempData["Success"] = $"Room {model.RoomNumber} updated successfully.";
            return RedirectToAction(nameof(Details), new { id });
        }

        // GET: /Rooms/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var model = await _roomService.GetForDeleteAsync(id);
            if (model is null) return NotFound();
            return View(model);
        }

        // POST: /Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deleted = await _roomService.DeleteAsync(id);
            if (!deleted) return NotFound();

            TempData["Success"] = "Room deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}