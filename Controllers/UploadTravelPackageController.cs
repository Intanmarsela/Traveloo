using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TravelWeb.Models;
using TravelWeb.Data;

[Authorize(Roles = "Agent")]
public class UploadTravelPackageController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IWebHostEnvironment _env;

    public UploadTravelPackageController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment env)
    {
        _context = context;
        _userManager = userManager;
        _env = env;
    }

    // GET: /Feed/UploadTravelPackage
    public IActionResult Create()
    {
        return View();
    }

    // POST: /Feed/UploadTravelPackage
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UploadTravelPackage(TravelPackage model, List<IFormFile> images)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var imageNames = new List<string>();

        foreach (var image in images)
        {
            if (image != null && image.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
                var path = Path.Combine(_env.WebRootPath, "images/agent_uploads", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                imageNames.Add(fileName);
            }
        }

        model.ImageUrls = string.Join(",", imageNames);

        var currentUser = await _userManager.GetUserAsync(User);
        model.AgentID = currentUser.Id;

        _context.TravelPackage.Add(model);
        await _context.SaveChangesAsync();

        return RedirectToAction("TravelPackage", "Home");
    }
}
