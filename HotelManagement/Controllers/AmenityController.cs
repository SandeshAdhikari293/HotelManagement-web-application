using HotelManagement.Data;
using HotelManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Controllers
{
    public class AmenityController : Controller
    {
        readonly ApplicationDbContext _context;
        public AmenityController(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var amenities = await _context.Amenities.ToListAsync();

            return View(amenities);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Add()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Add(Amenity amenity)
        {
            _context.Amenities.Add(amenity);
            _context.SaveChanges();
            var amenities = await _context.Amenities.ToListAsync();
            return View("Index", amenities);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(string? id)
        {
            Guid guid = Guid.Parse(id);
            Amenity amenity = _context.Amenities.Where(id => id.Id == guid).First();
            return View(amenity);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(Amenity amenity)
        {
            try
            {
                _context.Update(amenity);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AmenityExists(amenity.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            var amenities = await _context.Amenities.ToListAsync();
            return View("Index", amenities);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            Guid guid = Guid.Parse(id);
            Amenity amenity = await _context.Amenities.FindAsync(guid);
            if (amenity != null)
            {
                _context.Amenities.Remove(amenity);
                _context.SaveChanges();
            }

            var amenities = await _context.Amenities.ToListAsync();
            return View("Index", amenities);
        }


        private bool AmenityExists(Guid id)
        {
            return (_context.Amenities?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
