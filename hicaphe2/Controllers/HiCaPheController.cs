using hicaphe2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Web.UI;
using hicaphe2.Controllers.SearchIterator;

namespace hicaphe2.Controllers
{
    public class HiCaPheController : Controller
    {
        // Use DbContext to manage database
        private readonly HiCaPheEntities2 database = new HiCaPheEntities2();
        private List<SANPHAM> LaySanPhamMoi(int soluong)
        {
            return database.SANPHAMs.OrderByDescending(sanpham => sanpham.MaSP).Take(soluong).ToList();
        }
        // GET: BookStore
        public ActionResult Index(int? page)
        {
            int pageSize = 5;
            int pageNum = (page ?? 1);
            var dsSanPhamMoi = LaySanPhamMoi(5);
            return View(dsSanPhamMoi.ToPagedList(pageNum, pageSize));
        }

        public ActionResult SanPham(int? page, string timkiemchuoi, double minPrice = double.MinValue, double maxPrice = double.MaxValue)
        {
            var sanphams = database.SANPHAMs.ToList();        

            var iterator = new ProductIterator(sanphams);

            

            while (iterator.HasNext())
            {
                SANPHAM sanpham = iterator.Next();          
            }

            //Tìm kiếm chuỗi truy vấn theo NamePro, nếu chuỗi truy vấn SearchString khác rỗng, null
            if (!String.IsNullOrEmpty(timkiemchuoi) && timkiemchuoi.Trim().Length != 0)
            {
                String lowerCaseSearchText = timkiemchuoi.ToLower();               
                sanphams = sanphams.Where(s => s.TenSP.ToLower().Contains(timkiemchuoi)).ToList();
            }
            // Tìm kiếm chuỗi truy vấn theo đơn giá
            if (minPrice < maxPrice && minPrice >= 0 && maxPrice >= 0)
            {
                if (minPrice >= 0 && maxPrice > 0)
                {
                    sanphams = sanphams.OrderByDescending(x => x.Dongia).Where(p => (double)p.Dongia >= minPrice && (double)p.Dongia <= maxPrice).ToList();                   
                }
            }
            //Tạo biến cho biết số sách mỗi trang
            int pageSize = 7;
            //Tạo biến số trang
            int pageNum = (page ?? 1); 

            return View(sanphams.ToPagedList(pageNum, pageSize));
        }

        public ActionResult SPTheoLoai(int? page, int id)
        {
            int pageSize = 5;
            int pageNum = (page ?? 1);
            var dsSanPhamTheoLoai = database.SANPHAMs.Where(sanpham => sanpham.MaLoaiSP == id).ToList();
            return View("SPTheoLoai", dsSanPhamTheoLoai.ToPagedList(pageNum, pageSize));
        }

        public ActionResult LayLoaiSP() 
        {
            var dsLoai = database.LOAISPs.ToList();
            return PartialView(dsLoai);
        }

        public ActionResult Details(string id) 
        { 
            var sanpham= database.SANPHAMs.FirstOrDefault(s => s.MaSP == id);
            return View(sanpham);
        }
    }
}