using System.ComponentModel.DataAnnotations;

namespace Hotel.Dtos
{
    public class BannerDto
    {

        [Required(ErrorMessage = "Tiêu đề  không được để trống")]
        [MaxLength(100) ]
        public string? Title { get; set; }
        [MaxLength(100)]
        [Required(ErrorMessage = "Nội dung  không được để trống")]
        public string? Content { get; set; }

        [Required(ErrorMessage = "Ảnh  không được để trống")]
        public IFormFile? image {  get; set; }
    }
}
