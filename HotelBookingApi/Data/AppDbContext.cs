using HotelBooking.Domain.Models;
using HotelBookingApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingApi.Data
{
    public class AppDbContext :DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Agent> Agents { get; set; }
        public DbSet<Season> Seasons { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Season>()
       .Property(s => s.PriceFactor)
       .HasPrecision(10, 2); // 10 digits, 2 decimal places

            modelBuilder.Entity<Booking>()
                .Property(b => b.TotalPrice)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Room>()
                .Property(r => r.PricePerNight)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Room>()
                .HasIndex(r => new { r.RoomNumber, r.HotelId })
                .IsUnique();
        }

    }
}
