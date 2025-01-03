using EcomemerceASP_NET.Data;
using Microsoft.AspNetCore.Mvc;
using EcomemerceASP_NET.Helpers;
using EcomemerceASP_NET.ViewModels;
using System.Text.Json;

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


        public IActionResult Index()
        {
            return View(Cart);
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
            var couponid = db.Vouchers.FirstOrDefault(v => v.MaVc ==couponCode);  

            if (couponid != null)
            {
                return Json(new { message = "ok" });
            }
            else
            {
                return Json(new { message = "invalid" });
            }
        }

        #endregion
    }
}
