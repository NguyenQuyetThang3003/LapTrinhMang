using System.ComponentModel.DataAnnotations;

namespace BlogJavaJS.Models
{
    public class ContactViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập Họ")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập Tên")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập Email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập tin nhắn")]
        public string Message { get; set; } = string.Empty;
    }
}
