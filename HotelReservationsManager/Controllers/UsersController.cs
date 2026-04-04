using HotelReservationsManager.Models;
using HotelReservationsManager.Models.Domains;
using HotelReservationsManager.Models.ViewModels.User;
using HotelReservationsManager.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservationsManager.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;

        public UsersController(IUserService userService, UserManager<User> userManager)
        {
            _userService = userService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(UserFilterViewModel filters, int page = 1, int pageSize = 10)
        {
            var vm = await _userService.GetUsersAsync(filters, page, pageSize);
            return View(vm);
        }

        public async Task<IActionResult> Details(string id)
        {
            var vm = await _userService.GetUserDetailsAsync(id);
            return vm == null ? NotFound() : View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(DetailsUserViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var currentUserId = _userManager.GetUserId(User)!;

            ServiceResult result;
            try
            {
                result = await _userService.UpdateUserAsync(vm, currentUserId);
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Details), new { id = vm.Id });
            }

            if (!result.Succeeded)
            {
                // EGN or other uniqueness/validation errors go back to form
                ModelState.AddModelError(string.Empty, result.Error!);
                return View(vm);
            }

            TempData["Success"] = "User updated successfully.";
            return RedirectToAction(nameof(Details), new { id = vm.Id });
        }


        [HttpPost]
        public async Task<IActionResult> ToggleAdmin(string id)
        {
            try
            {
                var currentUserId = _userManager.GetUserId(User)!;
                await _userService.ToggleAdminAsync(id, currentUserId);
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
