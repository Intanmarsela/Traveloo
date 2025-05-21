using System.ComponentModel.DataAnnotations;

namespace TravelWeb.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        public ApplicationUser? User { get; set; }


        [Required]
        public string UserId { get; set; } // who booked it

        [Required]
        public int TravelPackageId { get; set; } // which package

        [Required]
        public int NumberOfPeople { get; set; } // how many people booked

        public DateTime BookingDate { get; set; } = DateTime.Now;
        public string? Review { get; set; }
        public int? StarRating { get; set; }
        public string? PaymentStatus { get; set; }
        public DateTime? InvoiceSentDate { get; set; }

        public bool? IsReviewHidden { get; set; } = false;

        public TravelPackage? TravelPackage { get; set; }
    }
}
