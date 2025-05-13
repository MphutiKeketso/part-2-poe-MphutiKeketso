using System;
using System.ComponentModel.DataAnnotations;
using EventEaseBookingSystem.Models;

public class Booking
{
    public int BookingId { get; set; }

    // Foreign Keys
    [Required]
    public int EventId { get; set; }

    [Required]
    public int VenueId { get; set; }

    [Required]
    public DateTime BookingDate { get; set; }

    // Navigation Properties (Make nullable)
    public virtual Event? Event { get; set; }  // Nullable
    public virtual Venue? Venue { get; set; }  // Nullable
}
