using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBookingSystem.Models
{
    public class Booking
    {
        public int BookingID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public int RoomID { get; set; }

        [Required]
        public DateTime CheckInDate { get; set; }

        [Required]
        public DateTime CheckOutDate { get; set; }

        //  Navigation properties
        public User? User { get; set; }
        public Room? Room { get; set; }

        //  NUMBER OF NIGHTS
        [NotMapped]
        public int NumberOfNights
        {
            get
            {
                var nights = (CheckOutDate - CheckInDate).Days;
                return nights > 0 ? nights : 0;
            }
        }

        //  TOTAL PRICE
        [NotMapped]
        public decimal TotalPrice
        {
            get
            {
                if (Room == null) return 0;

                return NumberOfNights * Room.Price;
            }
        }
    }
}