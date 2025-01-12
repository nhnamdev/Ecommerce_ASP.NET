using EcomemerceASP_NET.Data;
using Microsoft.AspNetCore.Mvc;
using EcomemerceASP_NET.Helpers;
using EcomemerceASP_NET.ViewModels;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace EcomemerceASP_NET.Controllers
{
    public class CartController : Controller
    {
        public readonly EcommerceContext db;
        public CartController(EcommerceContext context)
        {
            db = context;
        }

        public List<CartItem> Cart => HttpContext.Session.Get<List<CartItem>>(MySetting.CART_KEY) ?? new List<CartItem>();

        [Authorize]
        [HttpGet]
        public IActionResult Index()
        {
            if (Cart.Count == 0)
            {
                return Redirect("/");
            }
            return View(Cart);
        }
        [Authorize]
        [HttpGet]
        public IActionResult Checkout()
        {
            if (Cart.Count == 0)
            {
                return Redirect("/");
            }
            return View(Cart);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Checkout([FromBody] CheckoutVM model)
        {
            if (ModelState.IsValid)
            {
                var customerId = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CLAIM_CUSTOMERID)?.Value;

                if (string.IsNullOrEmpty(customerId))
                {
                    return Json(new { success = false, message = "Không tìm thấy thông tin khách hàng." });
                }

                var khachHang = new KhachHang();
                if (model.GiongKhacHangs)
                {
                    khachHang = db.KhachHangs.SingleOrDefault(kh => kh.MaKh == customerId);
                    if (khachHang == null)
                    {
                        return Json(new { success = false, message = "Thông tin khách hàng không tồn tại." });
                    }
                }

               
                var hoadon = new HoaDon
                {
                    MaVc = "VC000",
                    MaKh = string.IsNullOrEmpty(customerId) ? throw new ArgumentException("Mã khách hàng không hợp lệ.") : customerId,
                    HoTen = string.IsNullOrEmpty(model.Hoten) ? khachHang.HoTen : model.Hoten,
                    DiaChi = string.IsNullOrEmpty(model.DiaChi) ? khachHang.DiaChi : model.DiaChi,
                    //DienThoai = string.IsNullOrEmpty(model.DienThoai) ? khachHang.DienThoai : model.DienThoai,
                    NgayDat = DateTime.Now,
                    CachThanhToan = "COD",
                    CachVanChuyen = "Grad",
                    MaTrangThai = 0,
                    GhiChu = model.GhiChu ?? string.Empty
                };


                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.Add(hoadon);  
                        try
                        {
                            db.SaveChanges();
                        }
                        catch (DbUpdateException dbEx)
                        {
                            Console.WriteLine("Lỗi khi lưu dữ liệu: " + dbEx.InnerException?.Message);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Lỗi: " + ex.Message);
                        }

                        var cart = HttpContext.Session.Get<List<CartItem>>(MySetting.CART_KEY);
                        if (cart == null || !cart.Any())
                        {
                            return Json(new { success = false, message = "Giỏ hàng của bạn trống." });
                        }

                        var cthd = new List<ChiTietHd>();
                        foreach (var item in cart)
                        {
                            cthd.Add(new ChiTietHd
                            {
                                MaHd = hoadon.MaHd,
                                SoLuong = item.SoLuong,
                                DonGia = item.DonGia,
                                MaHh = item.MaHh,
                                GiamGia = 0
                            });
                        }

                        db.AddRange(cthd); 
                        db.SaveChanges(); 

                        HttpContext.Session.Set<List<CartItem>>(MySetting.CART_KEY, new List<CartItem>());
                        HttpContext.Session.Clear();

                        transaction.Commit(); 
                        return Json(new { success = true, message = "Đặt hàng thành công!" });
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();  
                        return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
                    }
                }

            }

            return Json(new { success = false, message = "Dữ liệu không hợp lệ." });
        }





        public IActionResult AddToCart(int id, int quantity = 1)
        {
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p => p.MaHh == id);
            if (item == null)
            {
                var hangHoa = db.HangHoas.SingleOrDefault(p => p.MaHh == id);
                if (hangHoa == null)
                {
                    TempData["Message"] = $"KLhoong tìm thấy hàng hóa có mã {id}";
                    return Redirect("/404");
                }
                item = new CartItem
                {
                    MaHh = hangHoa.MaHh,
                    TenHH = hangHoa.TenHh,
                    DonGia = hangHoa.DonGia ?? 0,
                    Hinh = hangHoa.Hinh ?? string.Empty,
                    SoLuong = quantity
                };
                gioHang.Add(item);

            }
            else
            {
                item.SoLuong += quantity;
            }

            HttpContext.Session.Set(MySetting.CART_KEY, gioHang);


            return RedirectToAction("Index");
        }
        public IActionResult RemoveCart(int id)
        {
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p => p.MaHh == id);
            if (item != null)
            {
                gioHang.Remove(item);
                HttpContext.Session.Set(MySetting.CART_KEY, gioHang);
            }
            return RedirectToAction("Index");
        }

        #region UpdateQuantity
        [HttpPost]
        public IActionResult UpdateQuantity(int productId, int quantity)
        {
            var cart = HttpContext.Session.Get<List<CartItem>>(MySetting.CART_KEY) ?? new List<CartItem>();
            var item = cart.SingleOrDefault(x => x.MaHh == productId);

            if (item != null)
            {
                item.SoLuong = quantity;
                HttpContext.Session.Set(MySetting.CART_KEY, cart);
            }
            else
            {
                return NotFound("Sản phẩm không tồn tại trong giỏ hàng!");
            }

            return Ok();
        }
        #endregion
        #region checkCoupon
        [HttpPost]
        public JsonResult checkCoupon([FromBody] JsonElement coupon)
        {
            var couponCode = coupon.GetProperty("couponCode").GetString();
            var couponid = db.Vouchers.FirstOrDefault(v => v.MaVc == couponCode);

            if (couponid != null)
            {
                return Json(new { discount = couponid.GiamGia / 100.0 });
            }
            else
            {
                return Json(new { discount = 0.0 });
            }
        }


        #endregion
        [Authorize]
        [HttpGet]
        public IActionResult GetCustomerInfo()
        {
            var customerId = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CLAIM_CUSTOMERID)?.Value;
            if (string.IsNullOrEmpty(customerId))
            {
                return Json(new { success = false, message = "Không tìm thấy khách hàng." });
            }

            var khachHang = db.KhachHangs.SingleOrDefault(kh => kh.MaKh == customerId);
            if (khachHang == null)
            {
                return Json(new { success = false, message = "Thông tin khách hàng không tồn tại." });
            }

            return Json(new
            {
                success = true,
                hoTen = khachHang.HoTen,
                diaChi = khachHang.DiaChi,
                dienThoai = khachHang.DienThoai
            });
        }


    }
}
