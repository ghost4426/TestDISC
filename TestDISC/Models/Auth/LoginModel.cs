using System;
using System.ComponentModel.DataAnnotations;

namespace TestDISC.Models.Auth
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Vui lòng điền email.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng điền mật khẩu.")]
        public string Password { get; set; }
    }
}