using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EcomemerceASP_NET.Data;

public partial class Contact
{
    public string? HoTen { get; set; }

    public string? Email { get; set; }

    public string NoiDung { get; set; } = null!;

    [Key]
    public int Id { get; set; }
}