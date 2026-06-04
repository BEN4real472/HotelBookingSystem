using System.ComponentModel.DataAnnotations;

namespace HotelBookingSystem.Models
{
    public class Room
    {
        [Key]
        public int RoomID { get; set; }

        public string RoomType { get; set; }

        public decimal Price { get; set; }

        public string Status { get; set; }
    }
}