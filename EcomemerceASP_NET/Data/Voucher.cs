using System;
using System.Collections.Generic;

namespace EcomemerceASP_NET.Data;

public partial class Voucher
{
    public string MaVc { get; set; } = null!;

    public int GiamGia { get; set; }

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();
}
