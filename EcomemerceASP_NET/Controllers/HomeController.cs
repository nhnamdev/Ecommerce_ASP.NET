using EcomemerceASP_NET.Data;
using EcomemerceASP_NET.Helpers;
using EcomemerceASP_NET.Models;
using EcomemerceASP_NET.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EcomemerceASP_NET.Controllers
{
    public class HomeController : Controller
    {
        private readonly EcommerceContext db;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, EcommerceContext context)
        {
            _logger = logger;
            db = context;
        }
        public List<CartItem> Cart => HttpContext.Session.Get<List<CartItem>>(MySetting.CART_KEY) ?? new List<CartItem>();

        public IActionResult Index()
        {
            var hangHoas = db.HangHoas.AsQueryable();

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
        [Route("/404")]
        public IActionResult PageNotFound()
        {
            return View();
        }
        [Route("/Contact")]
        public IActionResult Contact()
        {
            return View();
        }
        [Route("/Checkout")]
        public IActionResult Checkout()
        {
            return View(Cart);
        }
        [Route("/Testimonial")]
        public IActionResult Testimonial()
        {
            var gopYs = db.Gopies.AsQueryable();

            var result = gopYs.Select(p => new GopYVM
            {
                HoTen = p.HoTen,
                DienThoai = p.DienThoai,
                Email = p.Email,
                NoiDung = p.NoiDung
            });
            return View(result);
        
        }
     
    
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
