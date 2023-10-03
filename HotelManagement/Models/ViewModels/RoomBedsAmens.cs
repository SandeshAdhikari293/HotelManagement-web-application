namespace HotelManagement.Models.ViewModels
{
    public class RoomBedsAmens
    {
        public Room Room { get; set; }
        public List<Bed> Beds { get; set; }
        public List<Amenity> Amenities { get; set; }
    }
}
