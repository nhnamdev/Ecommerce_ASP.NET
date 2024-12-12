using Microsoft.AspNetCore.Mvc;
using EcomemerceASP_NET.Models;

using System.Diagnostics;
using EcomemerceASP_NET.Data;
using EcomemerceASP_NET.ViewModels;

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



            return View();
        }
        public IActionResult ManageProduct()
        {
            ViewBag.Title = "Product";



            return View();
        }
        public IActionResult ManageUser()
        {
            ViewBag.Title = "User";


            var khachHangs = db.KhachHangs.AsQueryable();



            var result = khachHangs.Select(p => new KhachHangVM
            {
                MaKh = p.MaKh,
                MatKhau = p.MatKhau,
                HoTen = p.HoTen,
                GioiTinh = p.GioiTinh,
                DiaChi = p.DiaChi,
                DienThoai = p.DienThoai,
                Email = p.Email,
                Hinh = p.Hinh,
                HieuLuc = p.HieuLuc,
                VaiTro = p.VaiTro
            });
            return View(result);
        }

        public IActionResult PaymentHistory()
        {
            ViewBag.Title = "Payment history";



            return View();
        }
    }
}
