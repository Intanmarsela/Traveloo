using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TravelWeb.Models;

namespace TravelWeb.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext (DbContextOptions options) : base(options)
        { 
        }
        public DbSet<TravelPackage> TravelPackage { get; set; }
        public DbSet<Booking> Bookings { get; set; } 


    }
}
