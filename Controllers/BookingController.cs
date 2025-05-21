using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelWeb.Data;
using TravelWeb.Models;


namespace TravelWeb.Controllers
{
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BookingController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        //Create booking for tourist to book the desire travel package
        [HttpGet]
        public IActionResult Create(int id) // id = TravelPackage ID
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Tourist")) //only tourist could book for travel package
            {
                return RedirectToAction("ErrorBooking");
            }

            var travelPackage = _context.TravelPackage.FirstOrDefault(tp => tp.Id == id);
            if (travelPackage == null)
            {
                return NotFound();
            }

            return View("Booking", travelPackage);//passing the TravelPackage ID to Booking view
        }
        //Post the fetch data to database
        [HttpPost]
        public IActionResult Create(int id, int numberOfPeople)
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Tourist"))
            {
                return RedirectToAction("ErrorBooking");
            }

            var userId = _userManager.GetUserId(User);
            var travelPackage = _context.TravelPackage.FirstOrDefault(tp => tp.Id == id);

            if (travelPackage == null || numberOfPeople <= 0 || numberOfPeople > (travelPackage.MaxGroupSize - travelPackage.CurrentBookings))
            {
                return BadRequest("Invalid booking request.");
            }

            var booking = new Booking
            {
                UserId = userId,
                TravelPackageId = id,
                NumberOfPeople = numberOfPeople
            };

            travelPackage.CurrentBookings += numberOfPeople; //update the number of current available 

            _context.Bookings.Add(booking);
            _context.SaveChanges();

            return RedirectToAction("Booked", new { id = booking.BookingId }); // Redirect to the Booked page for comfirmation
        }

        public IActionResult ErrorBooking()
        {
            return View();
        }

        public IActionResult Booked(int id)
        {
            var booking = _context.Bookings
                .Include(b => b.TravelPackage)
                .FirstOrDefault(b => b.BookingId == id);

            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

    }
}
