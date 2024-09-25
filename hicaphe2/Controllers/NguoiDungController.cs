using hicaphe2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace hicaphe2.Controllers
{
    public class NguoiDungController : Controller
    {
        private readonly HiCaPheEntities2 database = new HiCaPheEntities2();
        private readonly AuthenticationFacade _authFacade;

        public NguoiDungController()
        {
            database = new HiCaPheEntities2();
            _authFacade = new AuthenticationFacade(database);
        }
        // GET: NguoiDung
        [HttpGet]
        public ActionResult DangKy()
        {
            String taiKhoan = Session["TaiKhoan"] as String;
            if (taiKhoan != null)
                return RedirectToAction("Index","HiCaPhe");
            return View();
        }
        
        [HttpPost]
        public ActionResult DangKy(TAIKHOANKHACHHANG kh)
        {       

            if (ModelState.IsValid)
            {
                if (_authFacade.Register(kh.HoTenKH, kh.Email, kh.Matkhau, kh.SDT))
                    return RedirectToAction("DangNhap");
                ModelState.AddModelError(string.Empty, "Đã có người đăng ký email này");
            }
            return View();

        }
        [HttpGet]
        public ActionResult DangNhap()
        {           
            String taiKhoan = Session["TaiKhoan"] as String;
            if (taiKhoan != null)
                return RedirectToAction("Index","HiCaPhe");
            return View();

        }
        public ActionResult DangXuat()
        {
            Session["TaiKhoan"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("/DangNhap");
        }
        [HttpPost]
        public ActionResult DangNhap(TAIKHOANKHACHHANG kh)
        {         
            if (ModelState.IsValid)
            {
                if (_authFacade.Login(kh.Email, kh.Matkhau))
                {
                    var user = database.TAIKHOANKHACHHANGs.FirstOrDefault(k => k.Email == kh.Email && k.Matkhau == kh.Matkhau);
                    Session["TaiKhoan"] = user;
                    return RedirectToAction("Index", "HiCaPhe");
                }
                ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
        public ActionResult QuanLyDonHang()
        {
            // Lấy mã tài khoản của người đăng nhập
            int maTaiKhoan = ((TAIKHOANKHACHHANG)Session["TaiKhoan"]).MaTK;

            // Lấy danh sách đơn hàng của người đăng nhập từ cơ sở dữ liệu
            var danhSachDonHang = database.DONDATHANGs.Where(dh => dh.MaTK == maTaiKhoan).ToList();

            return View(danhSachDonHang);
        }
        public ActionResult HuyDonHang(int soDonHang)
        {
            // Lấy thông tin đơn hàng từ cơ sở dữ liệu
            var donHang = database.DONDATHANGs.SingleOrDefault(dh => dh.SODH == soDonHang);

            if (donHang == null)
            {
                return HttpNotFound();
            }

            // Kiểm tra xem đơn hàng đã được giao chưa
            if ((bool)donHang.Dagiao)
            {
                // Nếu đã giao, bạn có thể thực hiện các xử lý hoặc trả về trang thông báo tùy thuộc vào yêu cầu của bạn.
                // Ở đây chúng ta trả về trang thông báo để không thực hiện thêm hành động.
                ViewBag.Message = "Đơn hàng đã được giao và không thể hủy.";
                return View("ThongBao"); // Tạo view ThongBao.cshtml để hiển thị thông báo.
            }

            // Thực hiện xử lý hủy đơn hàng (cập nhật trạng thái, gửi thông báo, v.v.)
            donHang.Dagiao = false; // Đặt lại trạng thái là chưa giao
            donHang.Ngaygiaohang = null; // Đặt lại ngày giao hàng
            donHang.Dahuy = true; // Đặt trạng thái đã hủy

            // Lưu thay đổi vào cơ sở dữ liệu
            database.SaveChanges();

            // Thêm thông điệp vào TempData
            TempData["HuyDonHangSuccess"] = "Đơn hàng đã được hủy thành công.";

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
    
