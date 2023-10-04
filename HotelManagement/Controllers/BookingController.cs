using HotelManagement.Data;
using HotelManagement.Models;
using HotelManagement.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;


namespace HotelManagement.Controllers
{
    public class BookingController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        readonly ApplicationDbContext _context;
        public BookingController(UserManager<ApplicationUser> userManager, ApplicationDbContext applicationDbContext)
        {
            _userManager = userManager;
            _context = applicationDbContext;
        }
        public IActionResult Index()
        {
            var bookings = _context.Bookings.Include(r => r.Room).ToList();
            return View(bookings);
        }

        public IActionResult Add()
        {
            BookingSearch bookingSearch = new BookingSearch();
            bookingSearch.Rooms = new List<Room>(); // _context.Rooms.Include(a => a.Amenities).Include(b => b.Beds).ToList();

            return View(bookingSearch);
        }

        public IActionResult Search(Booking booking)
        {

            BookingSearch bookingSearch = new BookingSearch();
            bookingSearch.Booking = booking;


            bookingSearch.Rooms = new List<Room>();
            foreach (Room room in _context.Rooms.Include(a => a.Amenities).Include(b => b.Beds).ToList())
            {
                if(isAvailable(room, booking.CheckIn, booking.CheckOut))
                {
                    if (booking.People <= room.maxPeople())
                    {
                        bookingSearch.Rooms.Add(room);
                    }
                }
            }

            return View("Add", bookingSearch);
        }

        [HttpPost]
        public IActionResult Book(Booking booking, string roomID)
        {
            Guid guid = Guid.Parse(roomID);
            Room room = _context.Rooms.Find(guid);

            booking.Price = (int) (room.BasePrice * booking.People);
            booking.Room = room;
            string name = HttpContext.User.Identity.Name;
            booking.User = _userManager.Users.Where(u => u.Email.Equals(name)).First();

            _context.Bookings.Add(booking);
            _context.SaveChanges();

            return View("Success", booking);
        }

        public IActionResult Delete(string id)
        {
            Guid guid = Guid.Parse(id);
            Booking booking = _context.Bookings.Find(guid);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                _context.SaveChanges();
            }

            var bookings = _context.Bookings.Include(r => r.Room).ToList();
            return View("Index", bookings);
        }



        public bool isAvailable(Room room, DateTime checkin, DateTime checkout)
        {
            foreach (Booking booking in _context.Bookings.Include(r => r.Room).ToList())
            {
                if (booking.Room.Id == room.Id) 
                {
                    bool overlap = booking.CheckIn < checkout && checkin < booking.CheckOut;
                    if (overlap) return false;
                }
            }

            return true;
        }

        private bool BookingExists(Guid id)
        {
            return (_context.Bookings?.Any(e => e.Id == id)).GetValueOrDefault();
        }

    }
}
