using HotelReservationsManager.Models.Domains;
using Microsoft.AspNetCore.Identity;

namespace HotelReservationsManager.Middleware
{
    public class ActiveUserMiddleware
    {
        private readonly RequestDelegate _next;

        public ActiveUserMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var user = await userManager.GetUserAsync(context.User);
                if (user == null || !user.IsActive)
                {
                    await signInManager.SignOutAsync();
                    context.Response.Redirect("/Identity/Account/Login");
                    return;
                }

                // If their cookie claims are stale, refresh them
                var actualRoles = await userManager.GetRolesAsync(user);
                var claimRoles = context.User.Claims
                    .Where(c => c.Type == System.Security.Claims.ClaimTypes.Role)
                    .Select(c => c.Value)
                    .ToHashSet();

                if (!actualRoles.OrderBy(r => r).SequenceEqual(claimRoles.OrderBy(r => r)))
                {
                    await signInManager.RefreshSignInAsync(user);
                }
            }

            await _next(context);
        }
    }
}
