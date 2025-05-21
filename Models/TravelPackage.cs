using System.ComponentModel.DataAnnotations;
using TravelWeb.Models;

namespace TravelWeb.Models
{
    public class TravelPackage
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Summary { get; set; }
        public string? FullDescription { get; set; }
        public string? ImageUrls { get; set; }
        public string? AgentID { get; set; }

        public DateTime TravelDate { get; set; }
        public decimal Price { get; set; }
        public int MaxGroupSize { get; set; }
        public int CurrentBookings { get; set; } = 0;
        public ICollection<Booking>? Bookings { get; set; }


    }
}
