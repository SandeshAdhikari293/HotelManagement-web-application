namespace HotelManagement.Models
{
    public class Amenity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<Room> Rooms { get; set; }
    }
}
