using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using hicaphe2.Models;
using PagedList;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.Data.Entity;

namespace hicaphe2.Controllers
{
    public class AdminController : Controller
    {
        HiCaPheEntities2 database = new HiCaPheEntities2();
        
        // GET: Admin
        public ActionResult Index()
        {
            if (Session["Admin"] == null)
                return RedirectToAction("Login");
            return View();
        }

        [HttpGet] 
        public ActionResult Login() 
        {
            String taiKhoan = Session["Admin"] as String;
            if (taiKhoan != null)
                return Redirect("/HiCaPhe/Index");
            return View();
        }

        [HttpPost]
        public ActionResult Login(ADMIN admin)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(admin.UserAdmin))
                    ModelState.AddModelError(string.Empty, "User name không được để trống");
                if (string.IsNullOrEmpty(admin.PassAdmin))
                    ModelState.AddModelError(string.Empty, "Password không được để trống");
                    //Kiểm tra có admin này hay chưa
                    var adminDB = database.ADMINs.FirstOrDefault(ad => ad.UserAdmin == admin.UserAdmin && ad.PassAdmin == admin.PassAdmin);
                if (adminDB == null)
                    ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không đúng");
                else
                {
                    Session["Admin"] = adminDB;
                    ViewBag.ThongBao = "Đăng nhập admin thành công";
                    return RedirectToAction("Index", "Admin");
                }
            }
            return View();
        }

        public ActionResult SanPham(int? page, string timkiemchuoi, double minPrice = double.MinValue, double maxPrice = double.MaxValue)
        {
            var sanphams = database.SANPHAMs.ToList();
            //Tạo biến cho biết số sách mỗi trang
            int pageSize = 7;
            //Tạo biến số trang
            int pageNum = (page ?? 1);

            //Tìm kiếm chuỗi truy vấn theo NamePro, nếu chuỗi truy vấn SearchString khác rỗng, null
            if (!String.IsNullOrEmpty(timkiemchuoi) && timkiemchuoi.Trim().Length != 0)
            {
                String lowerCaseSearchText = timkiemchuoi.ToLower();
                sanphams = sanphams.Where(s => s.TenSP.ToLower().Contains(timkiemchuoi)).ToList();
            }
            // Tìm kiếm chuỗi truy vấn theo đơn giá
            if(minPrice < maxPrice && minPrice >=0 && maxPrice >= 0)
            {
                if (minPrice >= 0 && maxPrice > 0)
                {
                    sanphams = sanphams.OrderByDescending(x => x.Dongia).Where(p => (double)p.Dongia >= minPrice && (double)p.Dongia <= maxPrice).ToList();
                }
            }


            return View(sanphams.OrderBy(sanpham => sanpham.MaSP).ToPagedList(pageNum, pageSize));
        }

        [HttpGet]
        public ActionResult ThemSanPham()
        {
            ViewBag.MaLoaiSP = new SelectList(database.LOAISPs.ToList(), "MaLoaiSP", "TenLoaiSP");
            return View();
        }
        [HttpPost]
        public ActionResult ThemSanPham(SANPHAM sanpham, HttpPostedFileBase Hinhminhhoa)
        {
            ViewBag.MaLoaiSP = new SelectList(database.LOAISPs.ToList(), "MaLoaiSP", "TenLoaiSP");

            if (Hinhminhhoa == null)
            {
                ViewBag.ThongBao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {


                    var fileName = Path.GetFileName(Hinhminhhoa.FileName);

                    //Tao duong dan toi file
                    var path = Path.Combine(Server.MapPath("~/Images"), fileName);

                    //Kiem tra hinh da ton tai trong he thong chua
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.ThongBao = "Hình đã tồn tại";

                    }
                    else
                    {
                        Hinhminhhoa.SaveAs(path);
                    }
                    sanpham.Hinhminhhoa = fileName;

                    database.SANPHAMs.Add(sanpham);
                    database.SaveChanges();
                }
                return RedirectToAction("Sanpham");
            }
        }
        public ActionResult ChiTietSanPham(string id)
        {
            var sanpham = database.SANPHAMs.FirstOrDefault(s => s.MaSP == id);
            if (sanpham == null) // khong thay san pham
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sanpham);
        }

        // GET: Products/Edit/5
        [HttpGet]
        public ActionResult Edit(string id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            SANPHAM product = database.SANPHAMs.Find(id);

            if (product == null)
                return HttpNotFound();

            ViewBag.MaLoaiSP = new SelectList(database.LOAISPs, "MaLoaiSP", "TenLoaiSP", product.MaLoaiSP);
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaSP,TenSP,Kichthuoc,Donvitinh,Dongia,Mota,Hinhminhhoa,MaLoaiSP,Soluongban")] SANPHAM product, HttpPostedFileBase Hinhminhhoa)
        {
            if (ModelState.IsValid)
            {
                var fileName = Path.GetFileName(Hinhminhhoa.FileName);
                var path = Path.Combine(Server.MapPath("~/Images"), fileName);
                Hinhminhhoa.SaveAs(path);
                product.Hinhminhhoa = fileName;
                database.Entry(product).State = EntityState.Modified;
                database.SaveChanges();
                return RedirectToAction("SANPHAM");
            }
            ViewBag.MaLoaiSP = new SelectList(database.LOAISPs, "MaLoaiSP", "TenLoaiSP", product.MaLoaiSP);
            return View(product);
        }

        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SANPHAM product = database.SANPHAMs.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            SANPHAM product = database.SANPHAMs.Find(id);
            database.SANPHAMs.Remove(product);
            database.SaveChanges();
            return RedirectToAction("SANPHAM");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                database.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult QuanLyDonHang()
        {
            // Lấy danh sách đơn hàng từ cơ sở dữ liệu (hoặc bất kỳ logic nào phù hợp)
            var danhSachDonHang = database.DONDATHANGs.ToList();
            return View(danhSachDonHang);
        }
        public ActionResult ChiTietDonHang(int soDonHang)
        {
            // Lấy thông tin đơn hàng từ cơ sở dữ liệu
            var donHang = database.DONDATHANGs.SingleOrDefault(dh => dh.SODH == soDonHang);

            if (donHang == null)
            {
                return HttpNotFound();
            }

            return View(donHang);
        }
        public ActionResult XacNhanDonHang(int soDonHang)
        {
            var donHang = database.DONDATHANGs.SingleOrDefault(dh => dh.SODH == soDonHang);

            if (donHang == null)
            {
                return HttpNotFound();
            }

            // Kiểm tra xem đơn hàng đã được xác nhận chưa
            if ((bool)donHang.Dagiao)
            {
                // Nếu đã xác nhận, bạn có thể thực hiện các xử lý hoặc trả về trang thông báo tùy thuộc vào yêu cầu của bạn.
                return View(donHang);
            }

            // Cập nhật trạng thái đơn hàng
            donHang.Dagiao = true;
            donHang.Ngaygiaohang = DateTime.Now;

            // Lưu thay đổi vào cơ sở dữ liệu
            database.SaveChanges();
            // Thêm thông điệp vào TempData
            TempData["XacNhanDonHangSuccess"] = "Đơn hàng của quý khách đã được xác nhận.";
            // Chuyển hướng về trang quản lý đơn hàng hoặc trang chi tiết đơn hàng đã xác nhận
            return RedirectToAction("QuanLyDonHang");
        }
        public ActionResult HuyDonHang(int soDonHang)
        {
            var donHang = database.DONDATHANGs.SingleOrDefault(dh => dh.SODH == soDonHang);

            if (donHang == null)
            {
                return HttpNotFound();
            }

            if (donHang.Dahuy ?? false)
            {
                // Nếu đã xác nhận, bạn có thể thực hiện các xử lý hoặc trả về trang thông báo tùy thuộc vào yêu cầu của bạn.
                return View("DonHangDaHuy", donHang);
            }

            // Thực hiện xử lý hủy đơn hàng (cập nhật trạng thái, gửi thông báo, v.v.)
            donHang.Dagiao = false; // Đặt lại trạng thái là chưa giao
            donHang.Ngaygiaohang = null; // Đặt lại ngày giao hàng
            donHang.Dahuy = true; // Đặt trạng thái đã hủy

            // Lưu thay đổi vào cơ sở dữ liệu
            database.SaveChanges();

            // Thêm thông điệp vào TempData
            TempData["HuyDonHangSuccess"] = "Đơn hàng của quý khách đã bị từ chối.";

            // Chuyển hướng về trang quản lý đơn hàng hoặc trang chi tiết đơn hàng đã hủy
            return RedirectToAction("QuanLyDonHang");
        }

        private readonly DonHangCaretaker _caretaker = new DonHangCaretaker();
        public ActionResult KhoiPhucDonHang(int soDonHang)
        {
            var donHang = database.DONDATHANGs.FirstOrDefault(d => d.SODH == soDonHang);
            if (donHang != null && donHang.Dahuy.HasValue && donHang.Dahuy.Value)
            {         // Lưu trạng thái trước khi hủy vào Memento
                var memento = new DonHangMemento(donHang.Dahuy.Value); _caretaker.Backup(memento);
                // Cập nhật trạng thái của đơn hàng thành đã hủy
                donHang.Dahuy = false; database.SaveChanges();

            }
            // Chuyển hướng về trang quản lý đơn hàng hoặc trang chi tiết đơn hàng đã hủy
            return RedirectToAction("QuanLyDonHang");
        }
    }
}