namespace HotelManagement.Models
{
    public class Bed
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int MaxPeople { get; set; }
        public ICollection<Room> Rooms { get; set; }
    }
}
