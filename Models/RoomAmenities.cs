using Hotel.Models.Shared;

namespace Hotel.Models
{
    public class RoomAmenities : BaseEntity
    {
        public int RoomId { get; set; }
        public virtual Room Room { get; set; }

        public int AmenitiesId { get; set; }
        public virtual Amenities Amenities { get; set; }
    }
}
