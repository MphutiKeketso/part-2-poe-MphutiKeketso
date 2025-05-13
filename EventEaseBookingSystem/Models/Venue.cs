using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventEaseBookingSystem.Models;

public class Venue
{
    public int VenueId { get; set; }

    [Required]
    [StringLength(50)]
    public string? VenueName { get; set; }

    [Required]
    [StringLength(50)]
    public string? Location { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage ="Capacity must be greater than 0")]
    public int Capacity { get; set; }

    public string? ImageUrl { get; set; }   // Default empty string for ImageUrl

    [NotMapped]
    public IFormFile? ImageFile { get; set; }
    // Navigation Property for Events
    //public virtual ICollection<Event> Events { get; set; } = new List<Event>();  // Initialize as empty list

    // Constructor
    //public Venue()
   // {
    //    VenueName = string.Empty;  // Initialize with empty string
    //    Location = string.Empty;  // Initialize with empty string
   // }
}
