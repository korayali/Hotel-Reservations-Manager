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

        }

        private static void ConfigureUserEntity(ModelBuilder builder)
        {
            builder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.DisplayName).IsRequired();
                entity.Property(u => u.FirstName).IsRequired();
                entity.Property(u => u.LastName).IsRequired();
                entity.Property(u => u.EGN).IsRequired().HasMaxLength(10);
                entity.Property(u => u.HireDate).IsRequired();
                entity.Property(u => u.IsActive).IsRequired();

                entity.HasIndex(u => u.EGN).IsUnique();
                
                entity.HasMany(u => u.Reservations)
                      .WithOne(r => r.User)
                      .HasForeignKey(r => r.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }


    }
}
