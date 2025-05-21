using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TravelWeb.Models;
using TravelWeb.Data;
using Microsoft.EntityFrameworkCore;
namespace TravelWeb.Controllers;

public class HomeController : Controller
{
    //for the Travel Package home controller 
    private readonly ApplicationDbContext _context;
    private readonly ILogger<HomeController> _logger;
    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }
    //rendering the TravelPackage view
    public async Task<IActionResult> TravelPackage()
    {
        var packages = await _context.TravelPackage.ToListAsync();
        return View(packages);
    }

    public IActionResult Index()
    {
        var packages = _context.TravelPackage.ToList(); // Fetch all travel packages from the database
        return View(packages); 
    }

    public IActionResult About()
    {
        return View();
    }
    public IActionResult Policy()
    {
        return View();
    }
    public IActionResult ContactUs()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
