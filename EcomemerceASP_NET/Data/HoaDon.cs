﻿using System;
using System.Collections.Generic;

namespace EcomemerceASP_NET.Data;

public partial class HoaDon
{
    public int MaHd { get; set; }

    public string MaKh { get; set; } = null!;

    public string MaVc { get; set; } = null!;

    public DateTime NgayDat { get; set; }

    public DateTime? NgayCan { get; set; }

    public DateTime? NgayGiao { get; set; }

    public string? HoTen { get; set; }

    public string DiaChi { get; set; } = null!;

    public string CachThanhToan { get; set; } = null!;

    public string CachVanChuyen { get; set; } = null!;

    public double PhiVanChuyen { get; set; }

    public int MaTrangThai { get; set; }

    public string? GhiChu { get; set; }

    public virtual ICollection<ChiTietHd> ChiTietHds { get; set; } = new List<ChiTietHd>();

    public virtual TrangThai MaTrangThaiNavigation { get; set; } = null!;

    public virtual Voucher MaVcNavigation { get; set; } = null!;
}
