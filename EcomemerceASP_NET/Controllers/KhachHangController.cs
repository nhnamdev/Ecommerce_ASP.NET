using AutoMapper;
using EcomemerceASP_NET.Data;
using EcomemerceASP_NET.Helpers;
using EcomemerceASP_NET.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;

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

        #region Register
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
                    return RedirectToAction("DangNhap");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);

                }

            }
            return View();
        }
        #endregion
        #region Login  
        [HttpGet]
        public IActionResult DangNhap(string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> DangNhap(LoginVM model, string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            //if (model.UserName.Equals("admin") && model.Password.Equals("123"))
            //{
            //    return Redirect("/Admin");
            //}
            if (ModelState.IsValid)
            {
                var khachHang = db.KhachHangs.SingleOrDefault(kh => kh.MaKh == model.UserName);
                if (khachHang == null)
                {
                    ModelState.AddModelError("loi", "Không có khách hàng này");
                }
                else
                {
                    if (khachHang.VaiTro == 1)
                    {
                        return Redirect("/Admin");
                    }
                    else
                    {
                        if (!khachHang.HieuLuc)
                        {
                            ModelState.AddModelError("loi", "Tài khoản đã bị khóa. Vui lòng liên hệ Admin.");
                        }
                        else
                        {
                            if (khachHang.MatKhau != model.Password.ToMd5Hash(khachHang.RandomKey))
                            {
                                ModelState.AddModelError("loi", "Sai thông tin đăng nhập");
                            }
                            else
                            {
                                var claims = new List<Claim> {
                                new Claim(ClaimTypes.Email, khachHang.Email),
                                new Claim(ClaimTypes.Name, khachHang.HoTen),
                                new Claim("CustomerID", khachHang.MaKh),

								//claim - role động
								new Claim(ClaimTypes.Role, "Customer")
                            };

                                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                                await HttpContext.SignInAsync(claimsPrincipal);

                                if (Url.IsLocalUrl(ReturnUrl))
                                {
                                    return Redirect(ReturnUrl);
                                }
                                else
                                {
                                    return Redirect("/");
                                }
                            }
                        }
                    }
                }
            }
            return View();
        }
        #endregion
        #region Profile
        [Authorize]
        public IActionResult Profile()
        {
            //Chat gpt
            var customerId = User.Claims.FirstOrDefault(c => c.Type == "CustomerID")?.Value;
            if (string.IsNullOrEmpty(customerId))
            {
                return RedirectToAction("DangNhap");
            }

            var khachHang = db.KhachHangs.SingleOrDefault(kh => kh.MaKh == customerId);
            if (khachHang == null)
            {
                return RedirectToAction("DangNhap");
            }

            var profileVM = new ProfileVM
            {
                HoTen = khachHang.HoTen,
                Email = khachHang.Email,
                DienThoai = khachHang.DienThoai,
                DiaChi = khachHang.DiaChi,
                Hinh = khachHang.Hinh
            };

            //Chat gpt
            return View(profileVM);
        }
        #endregion

        [Authorize]
        public async Task<IActionResult> DangXuat()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
        #region EditProfile
        [HttpGet]
        public IActionResult EditProfile()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult EditProfile(ProfileVM model, IFormFile? Hinh)
        {
            var customerId = User.Claims.FirstOrDefault(c => c.Type == "CustomerID")?.Value;
            if (string.IsNullOrEmpty(customerId))
            {
                return RedirectToAction("DangNhap");
            }

            var khachHang = db.KhachHangs.SingleOrDefault(kh => kh.MaKh == customerId);
            if (khachHang == null)
            {
                return NotFound("Khách hàng không tồn tại.");
            }

            if (ModelState.IsValid)
            {

                try
                {
                    khachHang.HoTen = model.HoTen;
                    khachHang.Email = model.Email;
                    khachHang.DienThoai = model.DienThoai;
                    khachHang.DiaChi = model.DiaChi;

                    db.Update(khachHang);
                    db.SaveChanges();
                    // Cập nhật lại claims
                    var claims = new List<Claim> {
                        new Claim(ClaimTypes.Email, khachHang.Email),
                        new Claim(ClaimTypes.Name, khachHang.HoTen),
                        new Claim("CustomerID", khachHang.MaKh),
                        new Claim(ClaimTypes.Role, "Customer")
        };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    // Đăng nhập lại để làm mới claims
                    HttpContext.SignInAsync(claimsPrincipal);

                    return RedirectToAction("Profile");

                }
                catch
                {

                }
            }

            return View(model);
        }


        #endregion

        [HttpGet]
        public JsonResult CheckUsername( string userName)
        {
            var user = db.KhachHangs.FirstOrDefault(u => u.MaKh.Equals(userName));
            Console.WriteLine(userName);
            if (user != null)
            {
                return Json(new { message = "OK" });
            }
            else
            {
                return Json(new { message = "Tài khoản không tồn tại." });
            }
        }

    }
}
