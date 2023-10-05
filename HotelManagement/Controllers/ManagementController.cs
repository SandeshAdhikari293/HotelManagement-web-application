using HotelManagement.Data;
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

            return View(bookings);
        }
    }
}
