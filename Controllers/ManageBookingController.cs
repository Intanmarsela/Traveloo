using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelWeb.Data;
using TravelWeb.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

//only agent could manage the booking
[Authorize(Roles = "Agent")]
public class ManageBookingsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailSender _emailSender; // inject your email service

    public ManageBookingsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IEmailSender emailSender)
    {
        _context = context;
        _userManager = userManager;
        _emailSender = emailSender;
    }

    // List all bookings for agent's packages
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        var bookings = await _context.Bookings
            .Include(b => b.TravelPackage)
            .Include(b => b.User)
            .Where(b => b.TravelPackage.AgentID == user.Id)
            .ToListAsync();

        return View(bookings);
    }

    // Update Payment Status
    [HttpPost]
    public async Task<IActionResult> UpdatePaymentStatus(int bookingId, string newStatus)
    {
        var booking = await _context.Bookings.FindAsync(bookingId);
        if (booking != null)
        {
            booking.PaymentStatus = newStatus;
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    // Hide Review
    [HttpPost]
    public async Task<IActionResult> HideReview(int bookingId)
    {
        var booking = await _context.Bookings.FindAsync(bookingId);
        if (booking != null && !string.IsNullOrEmpty(booking.Review))
        {
            booking.IsReviewHidden = true;
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    // Unhide Review
    [HttpPost]
    public async Task<IActionResult> UnhideReview(int bookingId)
    {
        var booking = await _context.Bookings.FindAsync(bookingId);
        if (booking != null && !string.IsNullOrEmpty(booking.Review))
        {
            booking.IsReviewHidden = false;
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

}
