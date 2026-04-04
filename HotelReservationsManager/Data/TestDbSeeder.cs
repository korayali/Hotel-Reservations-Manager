using Bogus;
using HotelReservationsManager.Enums;
using HotelReservationsManager.Models.Domains;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HotelReservationsManager.Data
{
    public static class TestDbSeeder
    {
        private const int RngSeed = 42;
        private const int EmployeeCount = 10;
        private const int RoomCount = 50;
        private const int GuestCount = 200;
        private const int ReservationCount = 300;

        public static async Task SeedAsync(IServiceProvider services)
        {
            var db = services.GetRequiredService<HotelReservationsManagerDbContext>();
            var userManager = services.GetRequiredService<UserManager<User>>();
            var logger = services.GetRequiredService<ILogger<SeederMarker>>();

            logger.LogInformation("🌱 Starting test data seed...");

            await db.Database.MigrateAsync();

            // Idempotency check
            if (await db.Rooms.AnyAsync())
            {
                logger.LogInformation("✅ Test data already seeded.");
                return;
            }

            var faker = new Faker("en");
            Randomizer.Seed = new Random(RngSeed);

            // =====================================================
            // 1. Employees
            // =====================================================
            logger.LogInformation("[1/4] Employees...");

            var employees = new List<User>();

            for (int i = 0; i < EmployeeCount; i++)
            {
                var email = $"employee{i + 1}@hotel.dev";

                var existing = await userManager.FindByEmailAsync(email);
                if (existing != null)
                {
                    employees.Add(existing);
                    continue;
                }

                var employee = new User
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                    DisplayName = faker.Name.FullName(),
                    FirstName = faker.Name.FirstName(),
                    MiddleName = faker.Name.FirstName(),
                    LastName = faker.Name.LastName(),
                    EGN = faker.Random.ReplaceNumbers("##########"),
                    HireDate = DateOnly.FromDateTime(faker.Date.Past(5)),
                    IsActive = true
                };

                var result = await userManager.CreateAsync(employee, "Employee123!");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(employee, "Employee");
                    employees.Add(employee);
                }
            }

            logger.LogInformation("✔ Employees seeded: {Count}", employees.Count);

            // =====================================================
            // 2. Rooms
            // =====================================================
            logger.LogInformation("[2/4] Rooms...");

            var roomTypes = Enum.GetValues<RoomType>();

            var rooms = Enumerable.Range(1, RoomCount)
                .Select(i => new Room
                {
                    RoomNumber = 100 + i,
                    Capacity = faker.Random.Int(1, 5),
                    Type = faker.PickRandom(roomTypes),
                    IsFree = true,
                    PricePerAdult = faker.Random.Double(80, 300),
                    PricePerChild = faker.Random.Double(40, 150)
                })
                .ToList();

            await db.Rooms.AddRangeAsync(rooms);
            await db.SaveChangesAsync();

            // =====================================================
            // 3. Guests
            // =====================================================
            logger.LogInformation("[3/4] Guests...");

            var guests = new Faker<Guest>()
                .RuleFor(g => g.FirstName, f => f.Name.FirstName())
                .RuleFor(g => g.LastName, f => f.Name.LastName())
                .RuleFor(g => g.PhoneNumber, f => f.Phone.PhoneNumber())
                .RuleFor(g => g.Email, f => f.Internet.Email())
                .RuleFor(g => g.isAdult, f => f.Random.Bool(0.8f))
                .Generate(GuestCount);

            await db.Guests.AddRangeAsync(guests);
            await db.SaveChangesAsync();

            // =====================================================
            // 4. Reservations
            // =====================================================
            logger.LogInformation("[4/4] Reservations...");

            var reservations = new List<Reservation>();
            var reservationGuests = new List<ReservationGuest>();

            for (int i = 0; i < ReservationCount; i++)
            {
                var room = faker.PickRandom(rooms);
                var employee = faker.PickRandom(employees);

                var checkIn = faker.Date.Between(
                    DateTime.UtcNow.AddMonths(-6),
                    DateTime.UtcNow.AddMonths(3));

                var stayDays = faker.Random.Int(1, 7);
                var checkOut = checkIn.AddDays(stayDays);

                var adults = faker.Random.Int(1, room.Capacity);
                var children = faker.Random.Int(0, room.Capacity - adults);

                decimal totalPrice =
                    (decimal)(adults * room.PricePerAdult * stayDays) +
                    (decimal)(children * room.PricePerChild * stayDays);

                var reservation = new Reservation
                {
                    RoomId = room.Id,
                    UserId = employee.Id,
                    CheckInDate = checkIn,
                    CheckOutDate = checkOut,
                    HasBreakfast = faker.Random.Bool(0.5f),
                    IsAllInclusive = faker.Random.Bool(0.3f),
                    TotalPrice = totalPrice
                };

                reservations.Add(reservation);
            }

            await db.Reservations.AddRangeAsync(reservations);
            await db.SaveChangesAsync();

            foreach (var reservation in reservations)
            {
                var room = rooms.First(r => r.Id == reservation.RoomId);
                var guestCount = faker.Random.Int(1, room.Capacity);

                var selectedGuests = faker.PickRandom(guests, guestCount).ToList();

                foreach (var guest in selectedGuests)
                {
                    reservationGuests.Add(new ReservationGuest
                    {
                        ReservationId = reservation.Id,
                        GuestId = guest.Id
                    });
                }
            }

            await db.ReservationGuests.AddRangeAsync(reservationGuests);
            await db.SaveChangesAsync();

            logger.LogInformation("🎉 Test data seed complete.");
        }

        private class SeederMarker { }
    }
}