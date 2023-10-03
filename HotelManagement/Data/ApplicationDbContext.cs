using HotelManagement.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Room>? Rooms { get; set; }
        public DbSet<Bed>? Beds { get; set; }
        public DbSet<Amenity>? Amenities { get; set; }
        public DbSet<Booking>? Bookings { get; set; }

    }
}