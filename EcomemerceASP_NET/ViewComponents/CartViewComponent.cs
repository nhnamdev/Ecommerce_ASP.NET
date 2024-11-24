using EcomemerceASP_NET.Helpers;
using EcomemerceASP_NET.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EcomemerceASP_NET.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var count = HttpContext.Session.Get<List<CartItem>>(MySetting.CART_KEY) ?? new List<CartItem>();
            return View("CartPanel",new CartModel
            {
                Quantity = count.Sum(p => p.SoLuong),
                Total = count.Sum(p => p.SoLuong * p.DonGia)
            });
        }

    }
}
