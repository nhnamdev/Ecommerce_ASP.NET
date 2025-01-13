namespace EcomemerceASP_NET.ViewModels
{
    public class ProductVM
    {
        public int MaHh { get; set; }
        public string TenHH { get; set; }
        public string TenLoai { get; set; }
        public string? MoTaDonVi { get; set; }
        public double? DonGia { get; set; }
        public string? Hinh { get; set; }
        public DateTime NgaySx { get; set; }
        public double GiamGia { get; set; }
        public string? MoTa { get; set; }
        public string TenNcc { get; set; }
        public string MaNcc { get; set; } = null!;
        public int MaLoai { get; set; }
    }
}