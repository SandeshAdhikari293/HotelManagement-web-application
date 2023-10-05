namespace HotelManagement.Models.ViewModels
{
    public class BookingRooms
    {
        public IEnumerable<Booking> Bookings { get; set; }
        public IEnumerable<Room> Rooms { get; set; }
    }
}
