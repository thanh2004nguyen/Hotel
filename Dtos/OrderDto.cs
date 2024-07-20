using System.ComponentModel.DataAnnotations;

namespace Hotel.Dtos
{
    public class OrderDto
    {

        [Required(ErrorMessage = "Tên khách hàng không được để trống")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        public string? Phone { get; set; }
        public int RoomId { get; set; }
        public String? Message { get; set; }
        [Required(ErrorMessage = " không được để trống")]
        public string? Type { get; set; }
        public DateTime Date {  get; set; }
    }
}
