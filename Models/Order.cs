using Hotel.Dtos;
using Hotel.Models.Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel.Models
{
    public class Order: BaseEntity
    {
        [Required]
        public string? Name { get; set; }
        [Required]

        public string? Phone {  get; set; }
        [Required]
        public string Type { get; set; }
        [ForeignKey("Room")]
         public int RoomId { get; set; }
        public Room? Room { get; set; }
        public String? Message {  get; set; }
        public bool IsViewed { get; set; }
        public DateTime DayCheckin { get; set; }

        public Order()
        {
            IsViewed = false;
        }

        public Order (OrderDto data)
        {
            Name = data.Name;
            Phone = data.Phone;
            Message= data.Message;
            RoomId = data.RoomId;
            DayCheckin = data.Date;
            Type = data.Type;
        }
    }
}
