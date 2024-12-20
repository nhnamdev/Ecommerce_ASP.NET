using AutoMapper;
using EcomemerceASP_NET.Data;
using EcomemerceASP_NET.Helpers;
using EcomemerceASP_NET.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EcomemerceASP_NET.Controllers
{
    public class KhachHangController : Controller
    {
        private readonly EcommerceContext _db;
        private readonly IMapper _mapper;

        public KhachHangController(EcommerceContext context, IMapper mapper)
        {
            _db = context;
            _mapper = mapper;
        }

        #region Register
        [HttpGet]
        public IActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DangKy(RegisterVM model, IFormFile? hinh)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var khachHang = _mapper.Map<KhachHang>(model);
                khachHang.RandomKey = MyUtil.GenerateRandomKey();
                khachHang.MatKhau = model.MatKhau.ToMd5Hash(khachHang.RandomKey);
                khachHang.HieuLuc = true;
                khachHang.VaiTro = 0;

                if (hinh != null)
                {
                    khachHang.Hinh = MyUtil.UploadHinh(hinh, "KhachHang");
                }

                _db.KhachHangs.Add(khachHang);
                await _db.SaveChangesAsync();
                TempData["Success"] = "Đăng ký thành công!";

                return RedirectToAction("DangNhap");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }
        #endregion

        #region Login
        [HttpGet]
        public IActionResult DangNhap(string? returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DangNhap(LoginVM model, string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            if (ModelState.IsValid)
            {
                var khachHang = _db.KhachHangs.SingleOrDefault(kh => kh.MaKh == model.UserName); 
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
                                // Lưu thông tin người dùng vào session
                                HttpContext.Session.SetString("CustomerID", khachHang.MaKh);
                                HttpContext.Session.SetString("CustomerName", khachHang.HoTen);
                                HttpContext.Session.SetString("CustomerEmail", khachHang.Email);

                                var claims = new List<Claim> {
                            new Claim(ClaimTypes.Email, khachHang.Email),
                            new Claim(ClaimTypes.Name, khachHang.HoTen),
                            new Claim("CustomerID", khachHang.MaKh),
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
            var customerId = User.Claims.FirstOrDefault(c => c.Type == "CustomerID")?.Value;

            if (string.IsNullOrEmpty(customerId))
            {
                return RedirectToAction("DangNhap");
            }

            var khachHang = _db.KhachHangs.SingleOrDefault(kh => kh.MaKh == customerId);

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

            return View(profileVM);
        }

        [HttpGet]
        [Authorize]
        public IActionResult EditProfile()
        {
            var customerId = User.Claims.FirstOrDefault(c => c.Type == "CustomerID")?.Value;

            if (string.IsNullOrEmpty(customerId))
            {
                return RedirectToAction("DangNhap");
            }

            var khachHang = _db.KhachHangs.SingleOrDefault(kh => kh.MaKh == customerId);

            if (khachHang == null)
            {
                return NotFound("Khách hàng không tồn tại.");
            }

            var profileVM = new ProfileVM
            {
                HoTen = khachHang.HoTen,
                Email = khachHang.Email,
                DienThoai = khachHang.DienThoai,
                DiaChi = khachHang.DiaChi
            };

            return View(profileVM);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditProfile(ProfileVM model, IFormFile? hinh)
        {
            var customerId = User.Claims.FirstOrDefault(c => c.Type == "CustomerID")?.Value;

            if (string.IsNullOrEmpty(customerId))
            {
                return RedirectToAction("DangNhap");
            }

            var khachHang = _db.KhachHangs.SingleOrDefault(kh => kh.MaKh == customerId);

            if (khachHang == null)
            {
                return NotFound("Khách hàng không tồn tại.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                khachHang.HoTen = model.HoTen;
                khachHang.Email = model.Email;
                khachHang.DienThoai = model.DienThoai;
                khachHang.DiaChi = model.DiaChi;

                if (hinh != null)
                {
                    khachHang.Hinh = MyUtil.UploadHinh(hinh, "KhachHang");
                }

                _db.KhachHangs.Update(khachHang);
                await _db.SaveChangesAsync();

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, khachHang.Email),
                    new Claim(ClaimTypes.Name, khachHang.HoTen),
                    new Claim("CustomerID", khachHang.MaKh),
                    new Claim(ClaimTypes.Role, "Customer")
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync(claimsPrincipal);

                TempData["Success"] = "Cập nhật thông tin thành công!";
                return RedirectToAction("Profile");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Đã xảy ra lỗi: " + ex.Message);
                return View(model);
            }
        }
        #endregion

        [Authorize]
        public async Task<IActionResult> DangXuat()
        {
            HttpContext.Session.Remove("CustomerID");
            HttpContext.Session.Remove("CustomerName");
            HttpContext.Session.Remove("CustomerEmail");

            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
    }
}
