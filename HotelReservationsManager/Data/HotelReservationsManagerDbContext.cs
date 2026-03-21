using HotelReservationsManager.Models.Domains;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelReservationsManager.Data
{
    public class HotelReservationsManagerDbContext : IdentityDbContext<User>
    {
        public HotelReservationsManagerDbContext(DbContextOptions<HotelReservationsManagerDbContext> options)
            : base(options)
        {
        }

        public DbSet<Guest> Guests => Set<Guest>();
        public DbSet<Room> Rooms => Set<Room>();
        public DbSet<Reservation> Reservations => Set<Reservation>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            ConfigureUserEntity(builder);
            ConfigureGuestEntity(builder);

        }

        private static void ConfigureUserEntity(ModelBuilder builder)
        {
            builder.Entity<User>(entity =>
            {
                entity.Property(u => u.DisplayName).IsRequired();
                entity.Property(u => u.FirstName).IsRequired();
                entity.Property(u => u.LastName).IsRequired();
                entity.Property(u => u.EGN).IsRequired().HasMaxLength(10);
                entity.Property(u => u.HireDate).IsRequired();
                entity.Property(u => u.IsActive).IsRequired();

                entity.HasIndex(u => u.DisplayName);
                entity.HasIndex(u => u.EGN).IsUnique();

                entity.HasMany(u => u.Reservations)
                      .WithOne(r => r.User)
                      .HasForeignKey(r => r.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private static void ConfigureGuestEntity(ModelBuilder builder)
        {
            builder.Entity<Guest>(entity =>
            {
                entity.HasKey(g => g.Id);

                entity.Property(g => g.FirstName).IsRequired();
                entity.Property(g => g.LastName).IsRequired();
                entity.Property(g => g.Email).IsRequired();
                entity.Property(g => g.PhoneNumber).IsRequired();

                entity.HasIndex(g => g.Email).IsUnique();

                entity.HasMany(g => g.ReservationGuests)
                      .WithOne(rg => rg.Guest)
                      .HasForeignKey(rg => rg.GuestId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private static void ConfigureRoomEntity(ModelBuilder builder)
        {
            builder.Entity<Room>(entity =>
            {
                entity.HasKey(r => r.Id);

                entity.Property(r => r.Capacity).IsRequired();
                entity.Property(r => r.Type).IsRequired();
                entity.Property(r => r.IsFree).IsRequired();
                entity.Property(r => r.PricePerAdult).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(r => r.PricePerChild).IsRequired().HasColumnType("decimal(18,2)");
                entity.HasIndex(r => r.RoomNumber).IsUnique();

                entity.HasMany(r => r.Reservations)
                      .WithOne(res => res.Room)
                      .HasForeignKey(res => res.RoomId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private void ConfigureReservationEntity(ModelBuilder builder)
        {
            builder.Entity<Reservation>(entity =>
            {
                entity.HasKey(r => r.Id);

                entity.Property(r => r.RoomId).IsRequired();
                entity.Property(r => r.UserId).IsRequired();
                entity.Property(r => r.CheckInDate).IsRequired();
                entity.Property(r => r.CheckOutDate).IsRequired();
                entity.Property(r => r.HasBreakfast).IsRequired();
                entity.Property(r => r.IsAllInclusive).IsRequired();
                entity.Property(r => r.TotalPrice).IsRequired().HasColumnType("decimal(18,2)");

                entity.HasOne(r => r.Room)
                      .WithMany(room => room.Reservations)
                      .HasForeignKey(r => r.RoomId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(r => r.User)
                      .WithMany(user => user.Reservations)
                      .HasForeignKey(r => r.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(r => r.ReservationGuests)
                      .WithOne(rg => rg.Reservation)
                      .HasForeignKey(rg => rg.ReservationId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
