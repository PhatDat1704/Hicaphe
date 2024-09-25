using hicaphe2.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace hicaphe2.Controllers
{
    public interface IGioHangController
    {
        ActionResult BackupGioHang();
        ActionResult CapNhatMatHang(string MaSP, int SoLuong);
        ActionResult DatHang();
        ActionResult DongYDatHang();
        ActionResult GioHangPartial();
        ActionResult HienThiGioHang();
        ActionResult HoanThanhDonDatHang();
        ActionResult Index();
        List<MatHangMua> LayGioHang();
        ActionResult ThemSanPhamVaoGio(string MaSP);
        ActionResult UndoGioHang();
        ActionResult XoaMatHang(string MaSP);
    }
}