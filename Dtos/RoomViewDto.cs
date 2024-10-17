using Hotel.Models;

namespace Hotel.Dtos
{
	public class RoomViewDto
	{
		public List<Room>? ListRooms { get; set; }
		public List<RoomProperty>? ListPropertys { get; set; }
        public List<RoomTypeCountDto>? RoomTypeCounts { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int roomsCount { get; set; }
        public FilterRoomData FilterRoomData { get; set; }
    }
}
