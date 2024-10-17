using Hotel.Models.Shared;

namespace Hotel.Models
{
    public class RoomWithRoomProperty:BaseEntity
    {
        public int RoomId { get; set; }
        public virtual Room? Room { get; set; }

        // Thiết lập khóa ngoại đến RoomProperty
        public int RoomPropertyId { get; set; }
        public virtual RoomProperty? RoomProperty { get; set; }
    }
}
