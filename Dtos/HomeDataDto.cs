using Hotel.Models;
using System.ComponentModel.DataAnnotations;

namespace Hotel.Dtos
{
    public class HomeDataDto
    {
        [Required(ErrorMessage = "Loại không được để trống")]
        public string? Type { get; set; }
        public IFormFile? Image { get; set; }
        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        public string? Header { get; set; }
        [Required(ErrorMessage = "Nội dung không được để trống")]
        public string? Content { get; set; }

        public HomeDataDto() { }
        public HomeDataDto(HotelData data) {
            Type= data.Type;
            Image = null;
            Header = data.Header;
            Content = data.Content;
        }
    }
    
}
