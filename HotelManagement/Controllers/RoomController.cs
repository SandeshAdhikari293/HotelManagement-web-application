using HotelManagement.Data;
using HotelManagement.Models;
using HotelManagement.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;

namespace HotelManagement.Controllers
{
    public class RoomController : Controller
    {
        readonly ApplicationDbContext _context;
        public RoomController(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }
        public async Task<IActionResult> Index()
        {
            var rooms = await _context.Rooms
                .Include(b => b.Beds)
                .Include(a => a.Amenities)
                .ToListAsync();

            return View(rooms);
        }


        public async Task<IActionResult> Add() {

            RoomBedsAmens roomBedsAmens = new RoomBedsAmens();
            roomBedsAmens.Room = new Room();
            roomBedsAmens.Amenities = await _context.Amenities.ToListAsync();
            roomBedsAmens.Beds = await _context.Beds.ToListAsync();

            return View(roomBedsAmens);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Room room, String[] bedList, String[] amenList)
        {


            //Add all the selected beds into the model 
            foreach (String bedID in bedList)
            {
                Guid bedGUID = Guid.Parse(bedID);

                Bed bed = _context.Beds.Where(id => id.Id == bedGUID).First();
                room.Beds.Add(bed);
            }

            //Add all the selected amenities into the model 
            foreach (String amenID in amenList)
            {
                Guid amenGUID = Guid.Parse(amenID);

                Amenity amenity = _context.Amenities.Find(amenGUID);
                room.Amenities.Add(amenity);
            }

            _context.Rooms.Add(room);
            _context.SaveChanges();

            var rooms = await _context.Rooms
            .Include(b => b.Beds)
            .Include(a => a.Amenities)
            .ToListAsync();

            return View("Index", rooms);
        }

        public IActionResult Edit(string? id)
        {
            Room room =_context.Rooms
                .Include(b => b.Beds)
                .Include(a => a.Amenities)
                .Where(i => i.Id == Guid.Parse(id))
                .First();

            RoomBedsAmens roomBedsAmens = new RoomBedsAmens();
            roomBedsAmens.Room = room;
            roomBedsAmens.Amenities = _context.Amenities.ToList();
            roomBedsAmens.Beds = _context.Beds.ToList();

            return View(roomBedsAmens);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Room room, String[] bedList, String[] amenList)
        {

            Room roomToBeUpdated = _context.Rooms
                .Include(a => a.Amenities)
                .Include(b => b.Beds)
                .Where(i => i.Id == room.Id).First();

            roomToBeUpdated.Amenities.Clear();
            roomToBeUpdated.Beds.Clear();
            roomToBeUpdated.Name = room.Name;
            roomToBeUpdated.Description = room.Description;
            roomToBeUpdated.BasePrice = room.BasePrice;

            //roomToBeUpdated.Tags.Clear();

            foreach (String bedID in bedList)
            {
                Guid bedGUID = Guid.Parse(bedID);

                Bed bed = _context.Beds.Where(id => id.Id == bedGUID).First();
                roomToBeUpdated.Beds.Add(bed);
            }

            room.Amenities.Clear();
            //Add all the selected amenities into the model 
            foreach (String amenID in amenList)
            {
                Guid amenGUID = Guid.Parse(amenID);

                Amenity amenity = _context.Amenities.Find(amenGUID);
                roomToBeUpdated.Amenities.Add(amenity);
            }

            try
            {
                _context.Update(roomToBeUpdated);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomExists(room.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            var rooms = await _context.Rooms
            .Include(b => b.Beds)
            .Include(a => a.Amenities)
            .ToListAsync();

            return View("Index", rooms);
        }

        public async Task<IActionResult> Delete(string id)
        {

            Guid guid = Guid.Parse(id);
            Room room = await _context.Rooms.FindAsync(guid);
            if (room != null)
            {
                _context.Rooms.Remove(room);
                _context.SaveChanges();
            }

            var rooms = await _context.Rooms
            .Include(b => b.Beds)
            .Include(a => a.Amenities)
            .ToListAsync();

            return View("Index", rooms);
        }

        private bool RoomExists(Guid id)
        {
            return (_context.Rooms?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
