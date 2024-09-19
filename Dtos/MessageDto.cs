namespace Hotel.Dtos
{
	public class MessageDto
	{
		public int MessageId { get; set; }
		public string SenderId { get; set; }
		public string ReceiverId { get; set; }
		public string? Content { get; set; }
		public DateTime DateSend { get; set; }
	}
}
