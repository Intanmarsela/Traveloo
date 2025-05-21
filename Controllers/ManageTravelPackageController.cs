using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelWeb.Data;
using TravelWeb.Models;

//only agent could manage the travel package
namespace TravelWeb.Controllers
{
    [Authorize(Roles = "Agent")]
    public class ManageTravelPackageController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ManageTravelPackageController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Display all uploaded packages by the logged-in agent
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var packages = await _context.TravelPackage
                .Where(p => p.AgentID == user.Id)
                .ToListAsync();

            return View(packages);
        }

        // GET: Edit a travel package
        public async Task<IActionResult> Edit(int id)
        {
            var package = await _context.TravelPackage.FindAsync(id);
            if (package == null)
                return NotFound();

            return View(package);
        }

        // POST: Save edited travel package
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TravelPackage updatedPackage, List<IFormFile>? NewImages, List<string>? RemoveImages)
        {
            if (id != updatedPackage.Id)
                return NotFound();

            var package = await _context.TravelPackage.FindAsync(id);
            if (package == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                // 🛠 Remove selected images
                if (RemoveImages != null && RemoveImages.Any())
                {
                    var currentImages = package.ImageUrls?.Split(',').ToList() ?? new List<string>();
                    currentImages = currentImages.Except(RemoveImages).ToList();
                    package.ImageUrls = string.Join(",", currentImages);
                }

                // 🛠 Add new uploaded images
                if (NewImages != null && NewImages.Any())
                {
                    foreach (var image in NewImages)
                    {
                        if (image != null && image.Length > 0)
                        {
                            var fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
                            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/agent_uploads", fileName);

                            using (var stream = new FileStream(path, FileMode.Create))
                            {
                                await image.CopyToAsync(stream);
                            }

                            if (string.IsNullOrEmpty(package.ImageUrls))
                                package.ImageUrls = fileName;
                            else
                                package.ImageUrls += "," + fileName;
                        }
                    }
                }

                // 🛠 Update other fields
                package.Title = updatedPackage.Title;
                package.Summary = updatedPackage.Summary;
                package.FullDescription = updatedPackage.FullDescription;
                package.TravelDate = updatedPackage.TravelDate;
                package.Price = updatedPackage.Price;
                package.MaxGroupSize = updatedPackage.MaxGroupSize;

                _context.Update(package);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(updatedPackage);
        }

    }
}
