﻿using System.ComponentModel.DataAnnotations;

namespace Hotel.Dtos
{
    public class LoginDto

    {
        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        public string Password { get; set; }
    }
}
