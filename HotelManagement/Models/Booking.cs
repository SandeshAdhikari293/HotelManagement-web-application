using Microsoft.AspNetCore.Identity;

namespace HotelManagement.Models
{
    public class Booking
    {
        public Guid Id { get; set; }
        public Room Room { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int People {  get; set; }
        public double Price {  get; set; }
        public ApplicationUser User { get; set; }

    }
}
