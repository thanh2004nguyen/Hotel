namespace Hotel.Dtos
{
	public class OrderViewDto
	{

		public string Name { get; set; }
		public string Phone { get; set; }
		public string RoomType { get; set; }
		public string Message { get; set; }
		public DateTime Date { get; set; }
		public DateTime CheckInDate { get; set; }
		public int OrderId { get; set; }
		public string Type {  get; set; }
		public bool Isviewed { get; set; }

		public OrderViewDto()
		{
			Name = string.Empty;
			Phone = string.Empty;
			Isviewed = false;
			RoomType = string.Empty;
			Message = string.Empty;



		}
	}
}
