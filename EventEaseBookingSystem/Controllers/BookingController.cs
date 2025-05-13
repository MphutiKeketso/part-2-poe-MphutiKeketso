using EventEaseBookingSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;


public class BookingController : Controller
{
    private readonly AppDbContext _context;

    public BookingController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Booking
    public async Task<IActionResult> Index(string searchString)
    {
        var bookings = _context.Booking.Include(b => b.Event).Include(b => b.Venue).AsQueryable(); //THE ASQUERYABLE MIGHT BE A PROBLEM LATER

        if (!string.IsNullOrEmpty(searchString))
        {
            bookings = bookings.Where(b => b.Venue.VenueName.Contains(searchString) || b.Event.EventName.Contains(searchString));
        }
        return View(await bookings.ToListAsync());
    } 

    // GET: Booking/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound(); 

        var booking = await _context.Booking
            .Include(b => b.Event)
            .Include(b => b.Venue)
            .FirstOrDefaultAsync(m => m.BookingId == id);

        if (booking == null)
            return NotFound();

        return View(booking);
    }

    // GET: Booking/Create
    public IActionResult Create()
    {
        ViewBag.EventId = new SelectList(_context.Event, "EventId", "EventName");
        ViewBag.VenueId = new SelectList(_context.Venue, "VenueId", "VenueName");
        return View();
    }

    // POST: Booking/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create( Booking booking)
    {
        var selectedEvent = await _context.Event.FirstOrDefaultAsync(e => e.EventId == booking.EventId);

        if (selectedEvent == null)
        {
            ModelState.AddModelError("", "Selected event not found.");
            ViewData["Events"] = _context.Event.ToList();
            ViewData["Venues"] = _context.Venue.ToList();
            return View(booking);
        }

        // Check manually for double booking
        var conflict = await _context.Booking
            .Include(b => b.Event)
            .AnyAsync(b => b.VenueId == booking.VenueId &&
                           b.Event.EventDate.Date == selectedEvent.EventDate.Date);

        if (conflict)
        {
            TempData["ErrorMessage"] = "This Venue is already booked for that date";

            return RedirectToAction(nameof(Index));
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Booking created successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                // If database constraint fails (e.g., unique key violation), show friendly message
                TempData["ErrorMessage"] = "This Venue is already booked for that date";
              
                return RedirectToAction(nameof(Index));
              //  return View(booking);
            }
        }

        ViewData["Events"] = _context.Event.ToList();
        ViewData["Venues"] = _context.Venue.ToList();
        return View(booking);
    }

    // GET: Booking/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var booking = await _context.Booking.FindAsync(id);
        if (booking == null)
            return NotFound();

        ViewBag.EventId = new SelectList(_context.Event, "EventId", "EventName", booking.EventId);
        ViewBag.VenueId = new SelectList(_context.Venue, "VenueId", "VenueName", booking.VenueId);
        return View(booking);
    }

    // POST: Booking/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("BookingId,BookingDate,EventId,VenueId")] Booking booking)
    {
        if (id != booking.BookingId)
            return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(booking);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(booking.BookingId))
                    return NotFound();
                else
                    throw;
            }
            return RedirectToAction(nameof(Index));
        }

        ViewBag.EventId = new SelectList(_context.Event, "EventId", "EventName", booking.EventId);
        ViewBag.VenueId = new SelectList(_context.Venue, "VenueId", "VenueName", booking.VenueId);
        return View(booking);
    }

    // GET: Booking/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var booking = await _context.Booking
            .Include(b => b.Event)
            .Include(b => b.Venue)
            .FirstOrDefaultAsync(m => m.BookingId == id);

        if (booking == null)
            return NotFound();

        return View(booking);
    }

    // POST: Booking/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var booking = await _context.Booking.FindAsync(id);
        if (booking == null)
            return NotFound();

        _context.Booking.Remove(booking);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool BookingExists(int id)
    {
        return _context.Booking.Any(e => e.BookingId == id);
    }
}
