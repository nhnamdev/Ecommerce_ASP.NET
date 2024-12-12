using EcomemerceASP_NET.Data;
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

        public IActionResult Index()
        {
            var hangHoas = db.HangHoas.AsQueryable();

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
            return View();
        }
        [Route("/Testimonial")]
        public IActionResult Testimonial()
        {
            return View();
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
