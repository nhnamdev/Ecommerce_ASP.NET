using System;
using System.Collections.Generic;

namespace EcomemerceASP_NET.Data;

public partial class HangHoa
{
    public int MaHh { get; set; }

    public string TenHh { get; set; } = null!;

    public int MaLoai { get; set; }

    public string? MoTaDonVi { get; set; }

    public double? DonGia { get; set; }

    public string? Hinh { get; set; }

    public DateTime NgaySx { get; set; }

    public double GiamGia { get; set; }

    public string? MoTa { get; set; }

    public string MaNcc { get; set; } = null!;

    public virtual ICollection<ChiTietHd> ChiTietHds { get; set; } = new List<ChiTietHd>();

    public virtual Loai MaLoaiNavigation { get; set; } = null!;

    public virtual NhaCungCap MaNccNavigation { get; set; } = null!;
}
