using EcomemerceASP_NET.Data;
using EcomemerceASP_NET.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcomemerceASP_NET.Controllers
{
    public class AdminController : Controller
    {
        private readonly EcommerceContext db;
        public AdminController(EcommerceContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            ViewBag.Title = "Admin";
            return View();
        }
        public IActionResult DashboardTop()
        {
            ViewBag.Title = "Dashboard";
            var model = new DashboardTopAdminVM
            {
                Visitors = 1000,
                Subscribers = 100,
                Sales = 1000000,
                Orders = 100
            };
            return View(model);
        }

        public IActionResult ManageType()
        {
            ViewBag.Title = "Type";

            var Types = db.Loais.AsQueryable();
            var result = Types.Select(p => new LoaiVM
            {
                TenLoai = p.TenLoai,
                MoTa = p.MoTa,
                MaLoai = p.MaLoai
            }).ToList();

            return View(result);
        }

        [HttpPost]
        public JsonResult AddType(string Name, string notes)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                return Json(new { success = false, message = "Tên loại không được để trống." });
            }

            var loai = new Loai
            {
                TenLoai = Name,
                MoTa = notes
            };

            db.Loais.Add(loai);
            db.SaveChanges();

            return Json(new { success = true, message = "Thêm loại thành công." });
        }

        [HttpPost]
        public JsonResult UpdateType(int MaLoai, string type, string moTa)
        {
            var loai = db.Loais.FirstOrDefault(p => p.MaLoai == MaLoai);
            if (loai == null)
            {
                return Json(new { success = false, message = "Không tìm thấy loại cần cập nhật." });
            }

            loai.TenLoai = type;
            loai.MoTa = moTa;
            db.SaveChanges();

            return Json(new { success = true, message = "Cập nhật loại thành công." });
        }


        [HttpPost]
        public JsonResult DeleteType(int MaLoai)
        {
            var loai = db.Loais.FirstOrDefault(p => p.MaLoai == MaLoai);
            if (loai == null)
            {
                return Json(new { success = false, message = "Không tìm thấy loại cần xóa." });
            }

            db.Loais.Remove(loai);
            db.SaveChanges();

            return Json(new { success = true, message = "Loại đã được xóa thành công." });
        }

        public IActionResult ManageProduct()
        {
            ViewBag.Title = "Product";
            var Products = db.HangHoas.AsQueryable();
            var result = Products.Include(p => p.MaLoaiNavigation).Select(p => new ProductVM
            {
                TenHH = p.TenHh,
                TenLoai = p.MaLoaiNavigation.TenLoai,
                DonGia = p.DonGia,
                Hinh = p.Hinh,
            }).ToList();
            return View(result);
        }
        public IActionResult ManageUser()
        {
            ViewBag.Title = "User";
            var khachHangs = db.KhachHangs.AsQueryable();
            var result = khachHangs.Select(p => new KhachHangVM
            {
                HoTen = p.HoTen,
                NgaySinh = p.NgaySinh,
                DiaChi = p.DiaChi,
                DienThoai = p.DienThoai,
            }).ToList();
            return View(result);
        }

        public IActionResult PaymentHistory()
        {
            ViewBag.Title = "Payment history";
            var HoaDons = db.HoaDons.AsQueryable();
            var result = HoaDons.Include(hd => hd.ChiTietHds).Select(hd => new HoaDonVM
            {
                MaHd = hd.MaHd,
                HoTen = hd.HoTen,
                DonGia = hd.ChiTietHds.FirstOrDefault().DonGia,
                SoLuong = hd.ChiTietHds.FirstOrDefault().SoLuong
            }).ToList();
            return View(result);
        }
        public IActionResult QuanLyNhaCungCap()
        {
            ViewBag.Title = "Quan Ly Nha Cung Cap";
            var nhaCungCaps = db.NhaCungCaps.AsQueryable();
            var re = nhaCungCaps.Select(n => new NhaCungCapVM
            {
                MaNcc = n.MaNcc,
                TenCongTy = n.TenCongTy,
                Email = n.Email,
                DienThoai = n.DienThoai,
                DiaChi = n.DiaChi
            }).ToList();
            return View(re);
        }
        public IActionResult QuanLyVoucher()
        {
            ViewBag.Title = "Quan Ly Voucher";
            var vouchers = db.Vouchers.AsQueryable();
            var re = vouchers.Select(v => new VoucherVM
            {
                MaVc = v.MaVc,
                GiamGia = v.GiamGia
            }).ToList();
            return View(re);
        }
        public IActionResult QuanLyContact()
        {
            ViewBag.Title = "Quan Ly Contact";
            var contacts = db.Contacts.AsQueryable();
            var re = contacts.Select(v => new ContactVM
            {
                Id = v.Id,
                HoTen = v.HoTen,
                Email = v.Email,
                NoiDung = v.NoiDung
            }).ToList();
            return View(re);
        }
        [HttpPost]
        public IActionResult UpdateNhaCungCap(string maNcc, string TenCongTy, string Email, string DienThoai, string DiaChi, string moTa)
        {
            var nhacungcap = db.NhaCungCaps.FirstOrDefault(p => p.MaNcc == maNcc);
            if (nhacungcap == null)
            {
                return NotFound("Không tìm thấy loại cần cập nhật.");
            }
            nhacungcap.MaNcc = maNcc;
            nhacungcap.TenCongTy = TenCongTy;
            nhacungcap.Email = Email;
            nhacungcap.DienThoai = DienThoai;
            nhacungcap.DiaChi = DiaChi;
            nhacungcap.MoTa = moTa;
            db.SaveChanges();
            return RedirectToAction("QuanLyNhaCungCap");
        }

        [HttpPost]
        public IActionResult DeleteNhaCungCap(string maNhaCungCap)
        {
            var nhacc = db.NhaCungCaps.FirstOrDefault(p => p.MaNcc == maNhaCungCap);
            if (nhacc == null)
            {
                return NotFound("Không tìm thấy loại cần xóa.");
            }
            db.NhaCungCaps.Remove(nhacc);
            db.SaveChanges();
            return RedirectToAction("QuanLyNhaCungCap");
        }

        [HttpPost]
        public IActionResult UpdateVoucher(string maVoucher, int giamGia)
        {
            var voucher = db.Vouchers.FirstOrDefault(v => v.MaVc == maVoucher);
            if (voucher == null)
            {
                return NotFound("Không tìm thấy loại cần cập nhật.");
            }
            voucher.MaVc = maVoucher;
            voucher.GiamGia = giamGia;
            db.SaveChanges();
            return RedirectToAction("QuanLyVoucher");
        }

        [HttpPost]
        public IActionResult DeleteVoucher(string maVoucher)
        {
            var voucher = db.Vouchers.FirstOrDefault(v => v.MaVc == maVoucher);
            if (voucher == null)
            {
                return NotFound("Không tìm thấy loại cần xóa.");
            }
            db.Vouchers.Remove(voucher);
            db.SaveChanges();
            return RedirectToAction("QuanLyVoucher");
        }
    }
}