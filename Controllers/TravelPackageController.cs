using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelWeb.Data;
using TravelWeb.Models;

//listing all the uploaded travel packages in database
namespace TravelWeb.Controllers
{
    public class TravelPackageController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TravelPackageController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Details(int id)
        {
            var package = await _context.TravelPackage
                .Include(tp => tp.Bookings)
                    .ThenInclude(b => b.User)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (package == null)
            {
                return NotFound();
            }

            return View(package);
        }

    }
}
