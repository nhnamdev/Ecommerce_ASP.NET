using EcomemerceASP_NET.Data;
using EcomemerceASP_NET.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;

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
            var exisLoai = db.Loais.FirstOrDefault(t => t.TenLoai == Name);
            if(exisLoai != null)
            {
                return Json(new { success = false, message = "Tên loại đã tồn tại." });
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

        [HttpPost]
        public IActionResult UpdateProduct([FromBody] ProductVM product)
        {
            if (product == null || product.DonGia <= 0)
            {
                return Json(new { success = false, message = "Dữ liệu không hợp lệ." });
            }

            var existingProduct = db.HangHoas.FirstOrDefault(p => p.MaHh == product.MaHh);
            if (existingProduct == null)
            {
                return Json(new { success = false, message = "Không tìm thấy sản phẩm." });
            }

            // Cập nhật thông tin sản phẩm
            existingProduct.TenHh = product.TenHH;
            existingProduct.MaLoai = product.MaLoai;
            existingProduct.MoTaDonVi = product.MoTaDonVi;
            existingProduct.DonGia = product.DonGia;
            existingProduct.Hinh = product.Hinh;
            existingProduct.NgaySx = product.NgaySx;
            existingProduct.GiamGia = product.GiamGia;
            existingProduct.MoTa = product.MoTa;
            existingProduct.MaNcc = product.MaNcc;

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

        //xóa khách hàng
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

        //view lịch sử thanh toán
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

        //view nhà cung cấp
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
                DiaChi = n.DiaChi,
                MoTa = n.MoTa
            }).ToList();
            return View(re);
        }
        //them nhà cung cấp
        public IActionResult AddNhaCungCap(string supplierId, string companyName, string email, string phone, string address, string moTa)
        {
            if (string.IsNullOrWhiteSpace(supplierId) || string.IsNullOrWhiteSpace(companyName) || string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(email))
            {
                return Json(new { success = false, message = "Dữ liệu không hợp lệ." });
            }
            var existingNCC = db.NhaCungCaps.FirstOrDefault(v => v.MaNcc == supplierId);
            var existingTenCT = db.NhaCungCaps.FirstOrDefault(n => n.TenCongTy == companyName);
            if(existingNCC != null || existingTenCT != null)
            {
                return Json(new { success = false, message = "Mã nhà cung cấp hoặc công ty đã tồn tại." });
            }
            var NhaCC = new NhaCungCap
            {
                MaNcc = supplierId,
                TenCongTy = companyName,
                Email = email,
                DienThoai = phone,
                DiaChi = address,
                MoTa = moTa
            };
            db.NhaCungCaps.Add(NhaCC);
            db.SaveChanges();

            return Json(new { success = true, message = "Thêm nhà cung cấp thành công." });
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

        //view contact
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
        //xóa contact 
        [HttpPost]
        public IActionResult DeleteContact(int Id)
        {
            var contact = db.Contacts.FirstOrDefault(v => v.Id == Id);
            if (contact == null)
            {
                return Json(new { success = false, message = "Không tìm thấy contact cần xóa." });
            }

            db.Contacts.Remove(contact);
            db.SaveChanges();
            return Json(new { success = true, message = "Contact đã được xóa thành công." });
        }
        [HttpPost]
        public IActionResult UpdateNhaCungCap(string maNcc, string TenCongTy, string Email, string DienThoai, string DiaChi, string moTa)
        {
            var nhacungcap = db.NhaCungCaps.FirstOrDefault(p => p.MaNcc == maNcc);
            if (nhacungcap == null)
            {
                return NotFound("Không tìm thấy nhà cung cấp cần cập nhật.");
            }
            nhacungcap.TenCongTy = TenCongTy;
            nhacungcap.Email = Email;
            nhacungcap.DienThoai = DienThoai;
            nhacungcap.DiaChi = DiaChi;
            nhacungcap.MoTa = moTa;
            db.SaveChanges();
            return Json(new { success = true, message = "Cập nhật thành công!" });
        }
        //xóa nhà cung cấp
        [HttpPost]
        public IActionResult DeleteNhaCungCap(string maNhaCungCap)
        {
            var nhacc = db.NhaCungCaps.FirstOrDefault(p => p.MaNcc == maNhaCungCap);
            if (nhacc == null)
            {
                return Json(new { success = false, message = "Không tìm thấy Nhà cung cấp cần xóa." });
            }
            db.NhaCungCaps.Remove(nhacc);
            db.SaveChanges();
            return Json(new { success = true, message = "Nhà cung đã được xóa thành công." });
        }

        [HttpPost]
        public IActionResult UpdateVoucher(string maVoucher, int giamGia)
        {
            var voucher = db.Vouchers.FirstOrDefault(v => v.MaVc == maVoucher);
            if (voucher == null)
            {
                return Json(new { success = false, message = "Không tìm thấy Nhà cung cấp cần cập nhật." });
            }

            voucher.GiamGia = giamGia;
            db.SaveChanges();
            return Json(new { success = true, message = "Nhà cung đã được cập nhật thành công." });
        }

        [HttpPost]
        public IActionResult DeleteVoucher(string MaVc)
        {
            var voucher = db.Vouchers.FirstOrDefault(v => v.MaVc == MaVc);
            if (voucher == null)
            {
                return Json(new { success = false, message = "Không tìm thấy voucher cần xóa." });
            }

            db.Vouchers.Remove(voucher);
            db.SaveChanges();
            return Json(new { success = true, message = "Voucher đã được xóa thành công." });
        }

        //thêm voucher 
        [HttpPost]
        public JsonResult AddVoucher(string maVoucher, int giamGia)
        {
            if (string.IsNullOrWhiteSpace(maVoucher))
            {
                return Json(new { success = false, message = "Mã voucher không được để trống." });
            }
            var existingVoucher = db.Vouchers.FirstOrDefault(v => v.MaVc == maVoucher);
            if (existingVoucher != null)
            {
                return Json(new { success = false, message = "Mã voucher đã tồn tại." });
            }
            var voucher = new Voucher
            {
                MaVc = maVoucher,
                GiamGia = giamGia
            };

            db.Vouchers.Add(voucher);
            db.SaveChanges();

            return Json(new { success = true, message = "Thêm voucher thành công." });
        }
    }
}