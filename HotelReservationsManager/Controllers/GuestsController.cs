namespace HotelReservationsManager.Controllers
{
    using global::HotelReservationsManager.Models.ViewModels.Guest;
    using global::HotelReservationsManager.Services;
    using global::HotelReservationsManager.Services.Guests;
    using global::HotelReservationsManager.Services.Interfaces;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    namespace HotelReservationsManager.Controllers
    {
        [Authorize]
        public class GuestsController : Controller
        {
            private readonly IGuestService _guestService;
            private const int DefaultPageSize = 10;
            private const int DefaultReservationsPageSize = 5;

            public GuestsController(IGuestService guestService)
            {
                _guestService = guestService;
            }

            // GET: /Guests
            public async Task<IActionResult> Index(
                GuestFilterViewModel filters,
                int page = 1,
                int pageSize = DefaultPageSize)
            {
                if (page < 1) page = 1;

                var model = await _guestService.GetAllAsync(filters, page, pageSize);
                return View(model);
            }

            // GET: /Guests/Details/5
            public async Task<IActionResult> Details(int id, int page = 1)
            {
                var model = await _guestService.GetDetailsByIdAsync(id, page, DefaultReservationsPageSize);
                if (model is null) return NotFound();
                return View(model);
            }

            // GET: /Guests/Create
            public IActionResult Create() => View(new CreateGuestViewModel());

            // POST: /Guests/Create
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Create(CreateGuestViewModel model)
            {
                if (!ModelState.IsValid)
                    return View(model);

                var result = await _guestService.CreateAsync(model);

                if (!result.Succeeded)
                {
                    // Route the error to the specific field that caused it
                    var field = result.Error!.Contains("email") ? nameof(model.Email) : nameof(model.PhoneNumber);
                    ModelState.AddModelError(field, result.Error!);
                    return View(model);
                }

                TempData["Success"] = "Guest created successfully.";
                return RedirectToAction(nameof(Index));
            }

            // GET: /Guests/Edit/5
            public async Task<IActionResult> Edit(int id)
            {
                var model = await _guestService.GetForEditAsync(id);
                if (model is null) return NotFound();
                return View(model);
            }

            // POST: /Guests/Edit/5
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Edit(int id, EditGuestViewModel model)
            {
                if (id != model.Id) return BadRequest();

                if (!ModelState.IsValid)
                    return View(model);

                var result = await _guestService.UpdateAsync(model);

                if (!result.Succeeded)
                {
                    if (result.Error == "Guest not found.")
                        return NotFound();

                    var field = result.Error!.Contains("email") ? nameof(model.Email) : nameof(model.PhoneNumber);
                    ModelState.AddModelError(field, result.Error!);
                    return View(model);
                }

                TempData["Success"] = "Guest updated successfully.";
                return RedirectToAction(nameof(Details), new { id });
            }

            // GET: /Guests/Delete/5
            public async Task<IActionResult> Delete(int id)
            {
                var model = await _guestService.GetForDeleteAsync(id);
                if (model is null) return NotFound();
                return View(model);
            }

            // POST: /Guests/Delete/5
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeleteConfirmed(int id)
            {
                try
                {
                    await _guestService.DeleteAsync(id);
                    return RedirectToAction(nameof(Index));
                }
                catch (InvalidOperationException ex)
                {
                    TempData["Error"] = ex.Message;
                    return RedirectToAction(nameof(Delete), new { id });
                }
            }
        }
    }
}
