﻿using System.ComponentModel.DataAnnotations;

namespace EcomemerceASP_NET.ViewModels
{
    public class LoaiVM
    {
        public string TenLoai { get; set; } = null!;
        public string? MoTa { get; set; }
        public int MaLoai { get; set; }
    }
}