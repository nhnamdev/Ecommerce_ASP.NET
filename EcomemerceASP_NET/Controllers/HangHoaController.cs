using EcomemerceASP_NET.Data;
using EcomemerceASP_NET.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
                MaHh = p.MaHH,
                TenHH = p.TenHH,
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
                hangHoas = hangHoas.Where(p => p.TenHH.Contains(query));

            }
            var result = hangHoas.Select(p => new HangHoaVM
            {
                MaHh = p.MaHH,
                TenHH = p.TenHH,
                Hinh = p.Hinh ?? "",
                DonGia = p.DonGia ?? 0,
                MoTaNgan = p.MoTaDonVi ?? "",
                TenLoai = p.MaLoaiNavigation.TenLoai
            });

            return View(result);
        }
        public IActionResult Detail(int id)
        {
            var data = db.HangHoas.Include(P => P.MaLoaiNavigation).SingleOrDefault(p => p.MaHH == id);
               
            if (data == null)
            {
                TempData["Massage"] = $"Không tìm thấy sản phẩm {id}";
                return Redirect("/404");
            }
            var result = new ChiTietHangHoaVM
            {
                MaHh = data.MaHH,
                TenHH = data.TenHH,
                DonGia = data.DonGia ?? 0,
                ChiTiet = data.MoTa ?? "",
                DiemDanhGia = 5,
                MoTaNgan = data.MoTaDonVi ?? "",
                TenLoai = data.MaLoaiNavigation.TenLoai,
                SoLuongTon = 10,
                Hinh = data.Hinh ?? "",

            };
            return View(result);
        }
    }
}
