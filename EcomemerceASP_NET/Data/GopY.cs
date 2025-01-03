using System;
using System.Collections.Generic;

namespace EcomemerceASP_NET.Data;

public partial class GopY
{
    public string MaGy { get; set; } = null!;

    public string NoiDung { get; set; } = null!;

    public DateOnly NgayGy { get; set; }

    public string? HoTen { get; set; }

    public string? Email { get; set; }

    public string? DienThoai { get; set; }
}
