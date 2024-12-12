using Microsoft.AspNetCore.Mvc;
using EcomemerceASP_NET.Models;

using System.Diagnostics;
using EcomemerceASP_NET.Data;
using EcomemerceASP_NET.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace EcomemerceASP_NET.Controllers
{
    public class AdminController : Controller
    {
        private readonly EcommerceContext db;
        public AdminController(EcommerceContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            ViewBag.Title = "Admin";
            return View();
        }
        public IActionResult DashboardTop()
        {
            ViewBag.Title = "Dashboard";
            var model = new DashboardTopAdminVM
            {
                Visitors = 1000,
                Subscribers = 100,
                Sales = 1000000,
                Orders = 100
            };
            return View(model);
        }
        
        public IActionResult ManageType()
        {
            ViewBag.Title = "Type";

            var Types = db.Loais.AsQueryable();
            var result = Types.Select(p => new LoaiVM
            {
                TenLoai = p.TenLoai,
                MoTa = p.MoTa,
            }).ToList();

            return View(result);
        }
        public IActionResult ManageProduct()
        {
            ViewBag.Title = "Product";
            var Products = db.HangHoas.AsQueryable();
            var result = Products.Include(p => p.MaLoaiNavigation).Select(p => new ProductVM
            {
                TenHH = p.TenHH,
                TenLoai = p.MaLoaiNavigation.TenLoai,
                DonGia = p.DonGia,
                Hinh = p.Hinh,
            }).ToList();
            return View(result);
        }
        public IActionResult ManageUser()
        {
            ViewBag.Title = "User";
            var khachHangs = db.KhachHangs.AsQueryable();
            var result = khachHangs.Select(p => new KhachHangVM
            {
                HoTen = p.HoTen,
                NgaySinh = p.NgaySinh,
                DiaChi = p.DiaChi,
                DienThoai = p.DienThoai,
            }).ToList();
            return View(result);
        }

        public IActionResult PaymentHistory()
        {
            ViewBag.Title = "Payment history";
            var HoaDons = db.HoaDons.AsQueryable();
            var result = HoaDons.Include(hd => hd.ChiTietHds).Select(hd => new HoaDonVM
            {
                MaHd = hd.MaHd,
                HoTen = hd.HoTen,
                DonGia = hd.ChiTietHds.FirstOrDefault().DonGia,
                SoLuong = hd.ChiTietHds.FirstOrDefault().SoLuong
            }).ToList();
            return View(result);
        }
    }
}
