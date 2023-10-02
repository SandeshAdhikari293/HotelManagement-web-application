namespace HotelManagement.Models
{
    public class Room
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double BasePrice { get; set; }
        public List<Bed> Beds { get; set; }
        public List<Amenity> Amenities { get; set; }

        public int maxPeople()
        {
            int count = 0;
            foreach (Bed bed in Beds)
            {
                count = count + bed.MaxPeople;
            }
            return count;
        }
    }
}
