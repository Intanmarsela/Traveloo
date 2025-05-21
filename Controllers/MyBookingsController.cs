using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelWeb.Data;
using TravelWeb.Models;

//showing the tourist booking history

namespace TravelWeb.Controllers
{
    [Authorize(Roles = "Tourist")]
    public class MyBookingsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MyBookingsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            var bookings = await _context.Bookings
                .Include(b => b.TravelPackage)
                .Where(b => b.UserId == userId)
                .ToListAsync();

            return View(bookings);
        }
        //tourist could leave a review for the travel package they booked
        [HttpGet]
        public IActionResult LeaveReview(int id)
        {
            var booking = _context.Bookings
                .Include(b => b.TravelPackage)
                .FirstOrDefault(b => b.BookingId == id);

            if (booking == null || booking.Review != null)
            {
                return BadRequest();
            }

            return View(booking);
        }

        [HttpPost]

        public IActionResult LeaveReview(int id, string reviewText, int starRating)
        {
            var booking = _context.Bookings.FirstOrDefault(b => b.BookingId == id);

            if (booking == null || booking.Review != null)
            {
                return BadRequest();
            }

            booking.Review = reviewText;
            booking.StarRating = starRating;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}
