using HotelManagement.Data;
using HotelManagement.Models;
using HotelManagement.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace HotelManagement.Controllers
{
    public class BookingController : Controller
    {

        readonly ApplicationDbContext _context;
        public BookingController(ApplicationDbContext applicationDbContext)
        {
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


            _context.Bookings.Add(booking);
            _context.SaveChanges();

            return View("Success", booking);
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



    }
}
