using Hotel.Models;

namespace Hotel.Dtos
{
	public class RoomDto
	{
		public Room? Room { get; set; }
		public OrderDto? Order { get; set; }

		public List<RoomPolicy>? Policies { get; set; }
		public List<Comment>? Comments { get; set; }

	}
}
