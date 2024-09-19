namespace Hotel.Models
{
    public class Message
    {
        public int MessageId { get; set; }
        public string? Content { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public DateTime DateSend { get; set; }
    }
}
