using Hotel.Dtos;
using Hotel.Models.Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel.Models
{
    public class Booking: BaseEntity
    {
        [Required]
        public string? Name { get; set; }
		public string? Status { get; set; }
		[Required]
        public string? Phone {  get; set; }

        [ForeignKey("Room")]
        public int RoomId { get; set; }
        public Room? Room { get; set; }
        public String? Message {  get; set; } //yêu cầu
        public bool IsViewed { get; set; }
        public DateTime DayCheckin { get; set; }
        public DateTime DayCheckout { get; set; }
		[Required]
		public int UserId { get; set; }
        public Booking()
        {
            Status = "Pending"; 
        }

        public Booking(OrderDto data)
        {
            UserId = data.UserId;
            Status = data.Status;
            Name = data.Name;
            Phone = data.Phone;
            Message= data.Message;
            RoomId = data.RoomId;
            DayCheckin = data.DayCheckIn;
            DayCheckout = data.DayCheckOut;
        }
    }
}
