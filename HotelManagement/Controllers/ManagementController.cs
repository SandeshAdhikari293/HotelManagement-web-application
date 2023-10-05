using HotelManagement.Data;
using HotelManagement.Models;
using HotelManagement.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Controllers
{
    public class ManagementController : Controller
    {
        readonly ApplicationDbContext _context;
        public ManagementController(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var bookings = _context.Bookings.Include(r => r.Room).Include(u => u.User).ToList();
            var rooms = _context.Rooms.ToList();
            int people = maxPeople();
            int room = getRoomCount();

            ViewBag.MaxPeople = people;
            ViewBag.RoomCount = room;

            BookingRooms bookingRooms = new BookingRooms();
            bookingRooms.Bookings = bookings;
            bookingRooms.Rooms = rooms;

            return View(bookingRooms);
        }

        public int maxPeople()
        {
            int people = 0;

            foreach (Room room in _context.Rooms.Include(b => b.Beds).ToList())
            {
                foreach(Bed bed in room.Beds)
                {
                    people += bed.MaxPeople;
                }
            }

            return people;
        }

        public int getRoomCount()
        {
            int roomCount = 0;

            foreach (Room room in _context.Rooms.ToList())
            {
                roomCount ++;
            }

            return roomCount;
        }
    }
}
