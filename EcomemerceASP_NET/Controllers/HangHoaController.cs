﻿using EcomemerceASP_NET.Data;
using EcomemerceASP_NET.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace EcomemerceASP_NET.Controllers
{
    public class HangHoaController : Controller
    {
        private readonly EcommerceContext db;
        public HangHoaController(EcommerceContext context)
        {
            db = context;
        }
        public IActionResult Index(int? loai)
        {
            var hangHoas = db.HangHoas.AsQueryable();
            if ((loai.HasValue))
            {
                hangHoas = hangHoas.Where(p => p.MaLoai == loai.Value);

            }
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
        public IActionResult Search(string query)
        {
            var hangHoas = db.HangHoas.AsQueryable();
            if (query != null)
            {
                hangHoas = hangHoas.Where(p => p.TenHh.Contains(query));

            }
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
        public IActionResult Detail(int id)
        {
            var data = db.HangHoas.Include(P => P.MaLoaiNavigation).SingleOrDefault(p => p.MaHh == id);

            if (data == null)
            {
                TempData["Massage"] = $"Không tìm thấy sản phẩm {id}";
                return Redirect("/404");
            }
            var result = new ChiTietHangHoaVM
            {
                MaHh = data.MaHh,
                TenHH = data.TenHh,
                DonGia = data.DonGia ?? 0,
                ChiTiet = data.MoTa ?? "",
                DiemDanhGia = 5,
                MoTaNgan = data.MoTaDonVi ?? "",
                MoTa = data.MoTa ?? "",
                TenLoai = data.MaLoaiNavigation.TenLoai,
                SoLuongTon = 10,
                Hinh = data.Hinh ?? "",

            };
            return View(result);
        }

        #region themGiamGia
        public JsonResult themGiamGia([FromBody] JsonElement coupon)
        {
            var couponCode = coupon.GetProperty("couponCode").GetString();
            if (couponCode != null)
            {

                var couponid = db.Vouchers.FirstOrDefault(v => v.MaVc == couponCode);
                var hangHoas = db.HangHoas.AsQueryable();
                if (couponid != null)
                {
                    foreach (var item in hangHoas)
                    {
                        item.GiamGia = couponid.GiamGia;
                    }

                    return Json(new { message = "ok" });
                }
            }
            return Json(new { message = "invalid" });
        }
        #endregion


        #region GetDiscount
        [HttpPost]
        public JsonResult GetDiscount(string couponCode)
        {
            var voucher = db.Vouchers.FirstOrDefault(v => v.MaVc == couponCode);

            if (voucher != null)
            {
                return Json(new { success = true, discount = voucher.GiamGia });
            }
            else
            {
                return Json(new { success = false, message = "Mã giảm giá không hợp lệ." });
            }
        }

        #endregion
    }
}
