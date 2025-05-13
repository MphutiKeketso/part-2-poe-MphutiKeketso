using EventEaseBookingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Azure.Storage.Blobs;
using System;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

public class VenueController : Controller
{
    private readonly AppDbContext _context;

    public VenueController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Venue
    public async Task<IActionResult> Index()
    {
        


        return View(await _context.Venue.ToListAsync());
    }

    // GET: Venue/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var venue = await _context.Venue.FirstOrDefaultAsync(m => m.VenueId == id);
        if (venue == null)
            return NotFound();

        return View(venue);
    }

    // GET: Venue/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Venue/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create( Venue venue)
    {
        if (ModelState.IsValid)
        {
            if (venue.ImageFile != null)
            {
                var blobUrl = await UploadImageToBlobAsync(venue.ImageFile);
                venue.ImageUrl = blobUrl;
                _context.Venue.Add(venue);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Venue created successfully.";
                return RedirectToAction(nameof(Index));
            }


            
        }
        return View(venue);
    }

    // GET: Venue/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var existingVenue = await _context.Venue.FindAsync(id);
        if (existingVenue == null)
            return NotFound();

        return View(existingVenue);
    }

    // POST: Venue/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Venue venue)
    {
        if (id != venue.VenueId)
            return NotFound();

        // Debugging: Log the venue object data to check if the model is updated
        Console.WriteLine($"Venue Name: {venue.VenueName}, Location: {venue.Location}, Capacity: {venue.Capacity}, ImageUrl: {venue.ImageUrl}");

        if (ModelState.IsValid)
        {
            try
            {
                if (venue.ImageFile != null)
                {
                    var blobUrl = await UploadImageToBlobAsync(venue.ImageFile);
                    venue.ImageUrl = blobUrl;
                }
                _context.Venue.Update(venue); // Update the venue object
                await _context.SaveChangesAsync(); // Save changes to the database
                TempData["SuccessMessage"] = "Venue edited successfully.";
                
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VenueExists(venue.VenueId))
                    return NotFound();
                else
                    throw;
            }
            return RedirectToAction(nameof(Index)); // Redirect to the index page 
        }
        return View(venue);
    } 

    // GET: Venue/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var venue = await _context.Venue.FindAsync(id);
        if (venue == null)
            return NotFound();

        return View(venue);
    }

    // POST: Venue/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, Venue venue)
    {
        var currentVenue = await _context.Venue.FindAsync(id);

        if (currentVenue == null) return NotFound();

        var hasBooking = await _context.Booking.AnyAsync(b => b.VenueId == id);

        if (hasBooking)
        {
            TempData["ErrorMessage"] = "Cannot delete venue because it already has existing bookings.";
            return RedirectToAction(nameof(Index)); //THIS LINE MIGHT BE A PROBLEM, MIGHT NEED TO CHANGE DELETE TO INDEX
        }
        else
        {
            _context.Venue.Remove(currentVenue); // Update the venue object
            await _context.SaveChangesAsync(); // Save changes to the database
            TempData["SuccessMessage"] = "Venue deleted successfully.";
            return RedirectToAction(nameof(Index)); // Redirect to the index page
        }



        


    }


    private async Task<string> UploadImageToBlobAsync(IFormFile imageFile)
    {
        var connectionString = "DefaultEndpointsProtocol=https;AccountName=st10445280eventease;AccountKey=/rxKYiGasz3ya785HLVtSVOBEyff1g9L/YOJvXioT3akPSHz9se2eZGoy4nUL+ubNcPRVD6aKmJK+AStyjpwSg==;EndpointSuffix=core.windows.net";
        var containerName = "eventease";
        var blobServiceClient = new BlobServiceClient(connectionString);
        var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        var blobClient = containerClient.GetBlobClient(Guid.NewGuid()+ Path.GetExtension(imageFile.FileName));
        var blobHttpHeaders = new Azure.Storage.Blobs.Models.BlobHttpHeaders
        {
            ContentType = imageFile.ContentType
        };

        using (var stream = imageFile.OpenReadStream())
        {
            await blobClient.UploadAsync(stream, new Azure.Storage.Blobs.Models.BlobUploadOptions
            {
                HttpHeaders = blobHttpHeaders
            });
        }
        return blobClient.Uri.ToString();
    }




    private bool VenueExists(int id)
    {
        return _context.Venue.Any(e => e.VenueId == id);
    }
}
