using Hotel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel.Dtos
{
    public class CommentDto
    {
        [Required(ErrorMessage = "Tên không được để trống")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Nội dung không được để trống")]
        public string? Content { get; set; }
        public IFormFile? image { get; set; }
        [Required(ErrorMessage = "số sao không được để trống")]
        public int start { get; set; }
        [Required(ErrorMessage = "vui lòng chọn phòng")]
        public int RoomId { get; set; }
       
     
    }
}
