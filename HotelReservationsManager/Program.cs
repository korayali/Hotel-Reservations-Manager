using HotelReservationsManager.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddIdentityServices();
builder.Services.AddDbSeeder();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddEmailServices();
builder.Services.AddUserService();
builder.Services.AddGuestService();
builder.Services.AddRoomService();
builder.Services.AddReservationService();
builder.Services.AddRoomAvailabilityUpdateService();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseActiveUserCheck();

app.MapStaticAssets();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

await app.SeedDatabaseAsync();

app.Run();
