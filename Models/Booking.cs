using Hotel.Dtos;
using Hotel.Models.Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel.Models
{
    public class Booking: BaseEntity
    {
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string SmokingPreference { get; set; }
        public string BedPreference { get; set; }
        public string SpecialRequest { get; set; } // Thêm trường yêu cầu đặc biệt
        public decimal Price { get; set; }
        public DateTime CheckinDate { get; set; }
        public DateTime CheckoutDate { get; set; }
        public int TotalNights { get; set; }
        public int UserId { get; set; }
        public int RoomId { get; set; }
        [ForeignKey("RoomId")]
        public Room Room { get; set; }
    }
}
