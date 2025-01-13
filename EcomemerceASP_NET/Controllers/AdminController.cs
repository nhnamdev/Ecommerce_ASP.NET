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

        //view loại 
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

        //thêm loại 
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

        //cập nhật loại 
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

        //xóa loại 
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

        //view product 
        public IActionResult ManageProduct()
        {
            ViewBag.Title = "Product";

            // Lấy danh sách các loại sản phẩm (Type) từ database
            var types = db.Loais.Select(l => new LoaiVM
            {
                MaLoai = l.MaLoai,
                TenLoai = l.TenLoai
            }).ToList();
            ViewBag.Types = types;

            var sub = db.NhaCungCaps.Select(n => new NhaCungCapVM
            {
                MaNcc = n.MaNcc,
                TenCongTy = n.TenCongTy
            }).ToList();
            ViewBag.subs = sub;
            var products = db.HangHoas.Include(p => p.MaLoaiNavigation).Select(p => new ProductVM
            {
                MaHh = p.MaHh,
                TenHH = p.TenHh,
                TenLoai = p.MaLoaiNavigation.TenLoai,
                MoTaDonVi = p.MoTaDonVi,
                DonGia = p.DonGia,
                Hinh = p.Hinh,
            }).ToList();

            return View(products);
        }

        //thêm product 
        [HttpPost]
        public IActionResult AddProduct(string TenHh, int MaLoai, string MaNcc, double DonGia, string Hinh, DateTime NgaySx, double GiamGia, string MoTa, string MoTaDonVi)
        {
            if (string.IsNullOrWhiteSpace(MaNcc) || DonGia <= 0)
            {
                return Json(new { success = false, message = "Dữ liệu không hợp lệ." });
            }

            var product = new HangHoa
            {
                TenHh = TenHh,
                MaLoai = MaLoai,
                MoTaDonVi = MoTaDonVi,
                DonGia = DonGia,
                Hinh = Hinh,
                NgaySx = NgaySx,
                GiamGia = GiamGia,
                MoTa = MoTa,
                MaNcc = MaNcc
            };

            db.HangHoas.Add(product);
            db.SaveChanges();

            return Json(new { success = true, message = "Thêm sản phẩm thành công." });
        }
        //xóa product 
        [HttpPost]
        public IActionResult DeleteProduct(int productId)
        {
            var product = db.HangHoas.FirstOrDefault(p => p.MaHh == productId); // Giả sử 'MaHh' là ID sản phẩm trong database.

            if (product == null)
            {
                return Json(new { success = false, message = "Sản phẩm không tồn tại." });
            }

            // Xóa sản phẩm
            db.HangHoas.Remove(product);
            db.SaveChanges();

            return Json(new { success = true, message = "Sản phẩm đã được xóa thành công." });
        }

        [HttpGet]
        public JsonResult GetProductById(int productId)
        {
            var product = db.HangHoas.FirstOrDefault(p => p.MaHh == productId);
            if (product == null)
            {
                return Json(new { success = false, message = "Không tìm thấy sản phẩm." });
            }

            var productVM = new ProductVM
            {
                MaHh = product.MaHh,
                TenHH = product.TenHh,
                MaLoai = product.MaLoai,
                MoTaDonVi = product.MoTaDonVi,
                DonGia = product.DonGia,
                Hinh = product.Hinh,
                NgaySx = product.NgaySx,
                GiamGia = product.GiamGia,
                MoTa = product.MoTa,
                MaNcc = product.MaNcc
            };

            return Json(new { success = true, data = productVM });
        }

        [HttpPost]
        public JsonResult UpdateProduct(ProductVM productVM)
        {
            var product = db.HangHoas.FirstOrDefault(p => p.MaHh == productVM.MaHh);
            if (product == null)
            {
                return Json(new { success = false, message = "Không tìm thấy sản phẩm cần cập nhật." });
            }

            product.TenHh = productVM.TenHH;
            product.MaLoai = productVM.MaLoai;
            product.MoTaDonVi = productVM.MoTaDonVi;
            product.DonGia = productVM.DonGia;
            product.Hinh = productVM.Hinh;
            product.NgaySx = productVM.NgaySx;
            product.GiamGia = productVM.GiamGia;
            product.MoTa = productVM.MoTa;
            product.MaNcc = productVM.MaNcc;

            db.SaveChanges();
            return Json(new { success = true, message = "Cập nhật sản phẩm thành công." });
        }


        //view user 
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
                MaKh = p.MaKh
            }).ToList();
            return View(result);
        }
        [HttpPost]
        public JsonResult DeleteUser(string MaKh)
        {
            var user = db.KhachHangs.FirstOrDefault(p => p.MaKh == MaKh);
            if (user == null)
            {
                return Json(new { success = false, message = "Không tìm thấy khách hàng cần xóa." });
            }

            db.KhachHangs.Remove(user);
            db.SaveChanges();

            return Json(new { success = true, message = "khách hàng đã được xóa thành công." });
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