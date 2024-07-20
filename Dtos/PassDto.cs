using System.ComponentModel.DataAnnotations;

namespace Hotel.Dtos
{
    public class PassDto
    {
        [Required(ErrorMessage = "Mật khẩu hiện tại không được để trống")]
        public string? CurrentPassword { get; set; }

        [Required(ErrorMessage = "Mật khẩu mới không được để trống")]

        public string? NewPassword { get; set; }

        [Required(ErrorMessage = "Xác nhận mật khẩu không được để trống")]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu config sai")]
        public string? ConfirmPassword { get; set; }


    }
}