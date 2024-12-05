using AutoMapper;
using EcomemerceASP_NET.Data;
using EcomemerceASP_NET.Helpers;
using EcomemerceASP_NET.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EcomemerceASP_NET.Controllers
{
    public class KhachHangController : Controller
    {
        public readonly EcommerceContext db;
        public readonly IMapper _mapper;
        public KhachHangController(EcommerceContext context, IMapper mapper)
        {
            db = context;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public IActionResult DangKy(RegisterVM model, IFormFile Hinh)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var khachHang = _mapper.Map<KhachHang>(model);
                    khachHang.RandomKey = MyUtil.GenerateRandomKey();
                    khachHang.MatKhau = model.MatKhau.ToMd5Hash(khachHang.RandomKey);
                    khachHang.HieuLuc = true;
                    khachHang.VaiTro = 0;
                    if (Hinh != null)
                    {
                        khachHang.Hinh = MyUtil.UploadHinh(Hinh, "KhachHang");
                    }
                    db.Add(khachHang);
                    db.SaveChanges();
                    return RedirectToAction("Index", "HangHoa");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);

                }

            }
            return View();
        }
    }
}
