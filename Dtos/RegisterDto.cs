using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations;

namespace Hotel.Dtos
{
    public class RegisterDto
    {
        [Required(ErrorMessage ="Tên đăng nhập không được để trống")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [RegularExpression(@"^(?=.*\d)(?=.*[A-Z])(?=.*\W).+$", ErrorMessage = "Mật khẩu phải có ít nhât 1 số, 1 kí tự hoa và 1 kí tự đặc biệt")]
        public string? Pass { get; set; }
        [Required(ErrorMessage = "ui lòng chọn role để trống")]
        public string? Role { get; set; }
        [RegularExpression(@"^(?=.*\d)(?=.*[A-Z])(?=.*\W).+$", ErrorMessage = "Mật khẩu phải có ít nhât 1 số, 1 kí tự hoa và 1 kí tự đặc biệt")]
        [Compare("Pass", ErrorMessage = "xác nhận mật khẩu không đúng")]
        [Required(ErrorMessage = "xác nhận mật khẩu không được để trống")]
        public string? ConfirmPassword {  get; set; }

    }
}
