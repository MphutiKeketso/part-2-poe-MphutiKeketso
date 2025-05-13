using System;
using System.ComponentModel.DataAnnotations;

namespace EventEaseBookingSystem.Models;
public class Event
{
    public int EventId { get; set; }

    [Required]
    [StringLength(50)]
    public string EventName { get; set; }

    [Required]
    public DateTime EventDate { get; set; }

    [Required]
    public string Description { get; set; }

    // Foreign Key for Venue
    public int VenueId { get; set; }

    // Navigation Property
    public virtual Venue Venue { get; set; } = null!;  // Initialize with null!

    // Constructor
    public Event()
    {
        EventName = string.Empty;  // Initialize with empty string
        Description = string.Empty;  // Initialize with empty string
    }
}
