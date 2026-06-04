using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelBookingSystem.Models;

namespace HotelBookingSystem.Controllers
{
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        //  GET: Bookings
        public async Task<IActionResult> Index()
        {
            var bookings = _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Room);

            return View(await bookings.ToListAsync());
        }

        //  GET: Bookings/Create
        public IActionResult Create()
        {
            ViewData["UserID"] = new SelectList(_context.Users, "UserID", "Name");
            ViewData["RoomID"] = new SelectList(_context.Rooms, "RoomID", "RoomType");
            return View();
        }

        //  POST: Bookings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Booking booking)
        {
            //  Prevent past check-in
            if (booking.CheckInDate.Date < DateTime.Now.Date)
            {
                ModelState.AddModelError("CheckInDate", "Check-in date cannot be in the past.");
            }

            //  Prevent invalid date order
            if (booking.CheckOutDate.Date <= booking.CheckInDate.Date)
            {
                ModelState.AddModelError("CheckOutDate", "Check-out date must be after check-in date.");
            }

            if (ModelState.IsValid)
            {
                //  Check overlapping bookings
                var overlappingBooking = _context.Bookings
                    .FirstOrDefault(b =>
                        b.RoomID == booking.RoomID &&
                        booking.CheckInDate.Date < b.CheckOutDate.Date &&
                        booking.CheckOutDate.Date > b.CheckInDate.Date
                    );

                if (overlappingBooking != null)
                {
                    ModelState.AddModelError("RoomID",
                        $"This room is already booked from {overlappingBooking.CheckInDate:dd/MM/yyyy} to {overlappingBooking.CheckOutDate:dd/MM/yyyy}.");
                }
                else
                {
                    _context.Add(booking);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            //  Reload dropdowns
            ViewData["UserID"] = new SelectList(_context.Users, "UserID", "Name", booking.UserID);
            ViewData["RoomID"] = new SelectList(_context.Rooms, "RoomID", "RoomType", booking.RoomID);

            return View(booking);
        }

        //  GET: Bookings/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            ViewData["UserID"] = new SelectList(_context.Users, "UserID", "Name", booking.UserID);
            ViewData["RoomID"] = new SelectList(_context.Rooms, "RoomID", "RoomType", booking.RoomID);

            return View(booking);
        }

        //  POST: Bookings/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Booking booking)
        {
            if (id != booking.BookingID)
            {
                return NotFound();
            }

            //  Prevent past check-in
            if (booking.CheckInDate.Date < DateTime.Now.Date)
            {
                ModelState.AddModelError("CheckInDate", "Check-in date cannot be in the past.");
            }

            //  Prevent invalid date order
            if (booking.CheckOutDate.Date <= booking.CheckInDate.Date)
            {
                ModelState.AddModelError("CheckOutDate", "Check-out date must be after check-in date.");
            }

            if (ModelState.IsValid)
            {
                //  Check overlap (IGNORE CURRENT BOOKING)
                var overlappingBooking = _context.Bookings
                    .FirstOrDefault(b =>
                        b.RoomID == booking.RoomID &&
                        b.BookingID != booking.BookingID && // VERY IMPORTANT
                        booking.CheckInDate.Date < b.CheckOutDate.Date &&
                        booking.CheckOutDate.Date > b.CheckInDate.Date
                    );

                if (overlappingBooking != null)
                {
                    ModelState.AddModelError("RoomID",
                        $"This room is already booked from {overlappingBooking.CheckInDate:dd/MM/yyyy} to {overlappingBooking.CheckOutDate:dd/MM/yyyy}.");
                }
                else
                {
                    try
                    {
                        _context.Update(booking);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!_context.Bookings.Any(e => e.BookingID == booking.BookingID))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }

                    return RedirectToAction(nameof(Index));
                }
            }

            //  Reload dropdowns
            ViewData["UserID"] = new SelectList(_context.Users, "UserID", "Name", booking.UserID);
            ViewData["RoomID"] = new SelectList(_context.Rooms, "RoomID", "RoomType", booking.RoomID);

            return View(booking);
        }

        //  GET: Bookings/Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Room)
                .FirstOrDefaultAsync(m => m.BookingID == id);

            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        //  POST: Bookings/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}