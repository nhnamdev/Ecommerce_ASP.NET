using EcomemerceASP_NET.Data;
using EcomemerceASP_NET.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EcomemerceASP_NET.Controllers
{
    public class HangHoaController : Controller
    {
        private readonly EcommerceContext db;
        public HangHoaController(EcommerceContext context)
        {
            db = context;
        }
        public IActionResult Index(int? loai)
        {
            var hangHoas = db.HangHoas.AsQueryable();
            if ((loai.HasValue))
            {
                hangHoas = hangHoas.Where(p => p.MaLoai == loai.Value);

            }
            var result = hangHoas.Select(p => new HangHoaVM
            {
                MaHh = p.MaHh,
                TenHH = p.TenHh,
                Hinh = p.Hinh ?? "",
                DonGia = p.DonGia ?? 0,
                MoTaNgan = p.MoTaDonVi ?? "",
                TenLoai = p.MaLoaiNavigation.TenLoai
            });

            return View(result);
        }
        public IActionResult Search(string query)
        {
            var hangHoas = db.HangHoas.AsQueryable();
            if (query != null)
            {
                hangHoas = hangHoas.Where(p =>p.TenHh.Contains(query));

            }
            var result = hangHoas.Select(p => new HangHoaVM
            {
                MaHh = p.MaHh,
                TenHH = p.TenHh,
                Hinh = p.Hinh ?? "",
                DonGia = p.DonGia ?? 0,
                MoTaNgan = p.MoTaDonVi ?? "",
                TenLoai = p.MaLoaiNavigation.TenLoai
            });

            return View(result);
        }
    }
}
