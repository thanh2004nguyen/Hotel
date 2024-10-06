using Hotel.Models.Shared;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel.Models
{
    public class RoomAvailability : BaseEntity
    {
        [ForeignKey("Room")]
        public int RoomId { get; set; } // ID của phòng

        public virtual Room? Room { get; set; } // Liên kết với phòng

        [Required]
        public DateTime Date { get; set; } // Ngày

        public bool IsAvailable { get; set; } // Trạng thái phòng có sẵn trong ngày

        public RoomAvailability()
        {
            IsAvailable = true; // Mặc định là có sẵn
        }
    }
}
