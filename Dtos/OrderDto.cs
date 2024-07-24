using System.ComponentModel.DataAnnotations;

namespace Hotel.Dtos
{
    public class OrderDto
    {
        [Required(ErrorMessage = "Please login")]
        public int UserId { get; set; }
        [Required(ErrorMessage = "Please enter name")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "please enter phone number")]
        public string? Phone { get; set; }
		public string? Status { get; set; }
		public int RoomId { get; set; }
        public String? Message { get; set; }
        [Required(ErrorMessage = "please chooce type")]
        public string? Type { get; set; }
        [Required(ErrorMessage = "Please chooce day checkin")]
        public DateTime DayCheckIn {  get; set; }
        [Required(ErrorMessage = "Please chooce day checkout")]
        public DateTime DayCheckOut { get; set; }
    }
}
