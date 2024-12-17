using System;
using System.Collections.Generic;

namespace EcomemerceASP_NET.Data;

public partial class HoiDap
{
    public int MaHd { get; set; }

    public string CauHoi { get; set; } = null!;

    public string TraLoi { get; set; } = null!;

    public DateOnly NgayDua { get; set; }

    public string MaNv { get; set; } = null!;


}
