using Microsoft.AspNetCore.Identity;

namespace TravelWeb.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Add custom fields
        public string? DisplayName { get; set; }

    }
}
