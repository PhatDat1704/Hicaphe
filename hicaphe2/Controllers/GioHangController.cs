using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using hicaphe2.Models;

namespace hicaphe2.Controllers
{
    public class GioHangController : Controller
    {
        HiCaPheEntities2 database = new HiCaPheEntities2();
        public List<MatHangMua> LayGioHang()
        {
            List<MatHangMua> gioHang = Session["GioHang"] as List<MatHangMua>;
            if (gioHang == null)
            {
                gioHang = new List<MatHangMua>();
                Session["GioHang"] = gioHang;
            }
            return gioHang;
        }

        public ActionResult ThemSanPhamVaoGio(string MaSP)
        {
            List<MatHangMua> gioHang = LayGioHang();
            MatHangMua sanPham = gioHang.FirstOrDefault(s => s.MaSP == MaSP);
            if (sanPham == null)
            {
                sanPham = new MatHangMua(MaSP);
                gioHang.Add(sanPham);
            }
            else
            {
                sanPham.SoLuong++;
            }
            return RedirectToAction("Details", "HiCaPhe", new { id = MaSP });

        }

        private int TinhTongSL()
        {
            int tongSL = 0;
            List<MatHangMua> gioHang = LayGioHang();
            if (gioHang != null)
                tongSL = gioHang.Sum(sp => sp.SoLuong);
            return tongSL;
        }

        private double TinhTongTien()
        {
            double TongTien = 0;
            List<MatHangMua> gioHang = LayGioHang();
            if (gioHang != null)
                TongTien = gioHang.Sum(sp => sp.ThanhTien());
            return TongTien;
        }

        public ActionResult HienThiGioHang()
        {
            List<MatHangMua> gioHang = LayGioHang();
            if (gioHang == null || gioHang.Count == 0)
                return RedirectToAction("Index", "HiCaPhe");
            ViewBag.TongSL = TinhTongSL();
            ViewBag.TongTien = TinhTongTien();
            return View(gioHang);
        }
        public ActionResult GioHangPartial()
        {

            ViewBag.TongSL = TinhTongSL();
            ViewBag.TongTien = TinhTongTien();
            return PartialView();
        }

        public ActionResult XoaMatHang(string MaSP)
        {
            List<MatHangMua> gioHang = LayGioHang();
            var sanpham = gioHang.FirstOrDefault(s => s.MaSP == MaSP);
            if (sanpham != null)
            {
                gioHang.RemoveAll(s => s.MaSP == MaSP);
                return RedirectToAction("HienThiGioHang");
            }
            if (gioHang.Count == 0)
                return RedirectToAction("Index", "HiCaPhe");
            return RedirectToAction("HienThiGioHang");
        }

        public ActionResult CapNhatMatHang(string MaSP, int SoLuong)
        {
            List<MatHangMua> gioHang = LayGioHang();
            var sanpham = gioHang.FirstOrDefault(s => s.MaSP == MaSP);
            if (gioHang != null)
                sanpham.SoLuong = SoLuong;
            return RedirectToAction("HienThiGioHang");
        }

        public abstract class DonDatHangTemplate
        {
            protected abstract void TinhGiaTriDonHang();
            protected abstract void KiemTraTinhHopLe();
            protected abstract void XacNhanDonHang();

            public void XuLyDonDatHang()
            {
                KiemTraTinhHopLe();
                TinhGiaTriDonHang();
                XacNhanDonHang();
            }
        }

        public class DonDatHangCuaHang : DonDatHangTemplate
        {
            private List<MatHangMua> gioHang;
            private double tongTien;

            public DonDatHangCuaHang(List<MatHangMua> gioHang)
            {
                this.gioHang = gioHang;
            }

            protected override void TinhGiaTriDonHang()
            {
                tongTien = gioHang.Sum(sp => sp.ThanhTien());
            }

            protected override void KiemTraTinhHopLe()
            {
                if (gioHang == null || gioHang.Count == 0)
                    throw new Exception("Đơn hàng không hợp lệ");
            }

            protected override void XacNhanDonHang()
            {

                // Ví dụ: gửi email xác nhận, cập nhật trạng thái đơn hàng 
            }
        }

        public ActionResult DatHang()
        {
            if (Session["TaiKhoan"] == null)
                return RedirectToAction("DangNhap", "NguoiDung");
            List<MatHangMua> gioHang = LayGioHang();
            if (gioHang == null || gioHang.Count == 0)
                return RedirectToAction("Index", "HiCaPhe");
            ViewBag.TongSL = TinhTongSL();
            ViewBag.TongTien = TinhTongTien();
            return View(gioHang);
        }



        public ActionResult DongYDatHang()
        {
            TAIKHOANKHACHHANG khach = Session["TaiKhoan"] as TAIKHOANKHACHHANG;
            List<MatHangMua> gioHang = LayGioHang();
            DonDatHangCuaHang donDatHang = new DonDatHangCuaHang(gioHang);
            DONDATHANG DonHang = new DONDATHANG();
            DonHang.MaTK = khach.MaTK;
            DonHang.NgayDH = DateTime.Now;
            DonHang.Trigia = (decimal)TinhTongTien();
            DonHang.Dagiao = false;
            DonHang.Tennguoinhan = khach.HoTenKH;
            DonHang.Diachinhan = khach.DiachiKH;
            DonHang.Dienthoainhan = khach.SDT;
            DonHang.HTThanhtoan = false;
            DonHang.HTGiaohang = false;

            database.DONDATHANGs.Add(DonHang);
            database.SaveChanges();

            foreach (var sanpham in gioHang)
            {
                CTDATHANG chitiet = new CTDATHANG();
                chitiet.SODH = DonHang.SODH;
                chitiet.MaSP = sanpham.MaSP;
                chitiet.Soluong = sanpham.SoLuong;
                chitiet.Dongia = (decimal)sanpham.Dongia;
                database.CTDATHANGs.Add(chitiet);
            }
            database.SaveChanges();

            donDatHang.XuLyDonDatHang();

            //Xóa giỏ hàng
            Session["GioHang"] = null;
            return RedirectToAction("HoanThanhDonDatHang");
        }

        public ActionResult HoanThanhDonDatHang()
        {
            return View();
        }
        public ActionResult Index()
        {
            return View();
        }
    }
}