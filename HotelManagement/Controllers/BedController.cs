using HotelManagement.Data;
using HotelManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Controllers
{
    public class BedController : Controller
    {

        readonly ApplicationDbContext _context;
        public BedController(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var beds = await _context.Beds.ToListAsync();

            return View(beds);
        }

        public IActionResult Add() { 
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Bed bed)
        {
            _context.Beds.Add(bed);
            _context.SaveChanges();
            var beds = await _context.Beds.ToListAsync();
            return View("Index", beds);
        }

        public IActionResult Edit(string? id)
        {
            Guid guid = Guid.Parse(id);
            Bed bed = _context.Beds.Where(id => id.Id == guid).First();
            return View(bed);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Bed bed)
        {
            //Bed bedToBeUpdated = _context.Beds.Where(id => id.Id == bed.Id).First();
            try
            {
                _context.Update(bed);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BedExists(bed.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            var beds = await _context.Beds.ToListAsync();
            return View("Index", beds);
        }

        public async Task<IActionResult> Delete(string id)
        {
            Guid guid = Guid.Parse(id);
            Bed bed = await _context.Beds.FindAsync(guid);
            if (bed != null)
            {
                _context.Beds.Remove(bed);
                _context.SaveChanges();
            }
            
            var beds = await _context.Beds.ToListAsync();
            return View("Index", beds);
        }

        private bool BedExists(Guid id)
        {
            return (_context.Beds?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
