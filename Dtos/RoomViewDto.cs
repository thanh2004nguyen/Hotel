using Hotel.Models;

namespace Hotel.Dtos
{
	public class RoomViewDto
	{
		public List<Room?> ListRoom { get; set; }
		public List<RoomProperty> ListProperty { get; set; }
	}
}
