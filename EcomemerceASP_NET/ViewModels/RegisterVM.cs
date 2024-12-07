using System.ComponentModel.DataAnnotations;

namespace EcomemerceASP_NET.ViewModels
{
    public class RegisterVM
    {
        [Key]
        [Display(Name = "Tên đăng nhập")]
        [Required(ErrorMessage = "Mã khách hàng không được để trống")]
        [MaxLength(20, ErrorMessage = "Mã khách hàng không được quá 20 ký tự")]
        public string MaKh { get; set; }
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [Display(Name = "Mật Khẩu")]
        [DataType(DataType.Password)]
        public string MatKhau { get; set; }
        [Display(Name = "Họ tên")]
        public string HoTen { get; set; }

        public bool GioiTinh { get; set; } = true;
        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime? NgaySinh { get; set; }
        [MaxLength(60, ErrorMessage = "Địa chỉ không được quá 60 ký tự")]
        [Display(Name ="Địa chỉ")]
        public string DiaChi { get; set; }
        [MaxLength(20, ErrorMessage = "Điện thoại không được quá 20 ký tự")]
        [Display(Name = "Điện Thoại")]
        public string DienThoai { get; set; }
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; } = null!;

        public string? Hinh { get; set; }
    }
}
