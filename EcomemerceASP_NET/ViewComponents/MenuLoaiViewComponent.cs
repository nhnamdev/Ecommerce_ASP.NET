using EcomemerceASP_NET.Data;
using EcomemerceASP_NET.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EcomemerceASP_NET.ViewComponents
{
    public class MenuLoaiViewComponent : ViewComponent
    {
        private readonly EcommerceContext db;
        public MenuLoaiViewComponent(EcommerceContext context) => db = context;

        public IViewComponentResult Invoke()
        {
            var data = db.Loais.Select(lo => new MenuLoaiVM
            {
               MaLoai=  lo.MaLoai,
               TenLoai = lo.TenLoai,
               Soluong = lo.HangHoas.Count
            }).OrderBy(p => p.TenLoai);
            return View(data);
        }
    }
}
