using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hicaphe2.Models
{
    public class ProductPrototype
    {
        private static ProductPrototype instance;
        private readonly HiCaPheEntities2 database = new HiCaPheEntities2();
        private List<SANPHAM> products;

        private ProductPrototype()
        {
            products = database.SANPHAMs.ToList();
        }

        public static ProductPrototype GetInstance()
        {
            if (instance == null)
            {
                instance = new ProductPrototype();
            }
            return instance;
        }

        public List<SANPHAM> Products
        {
            get { return products; }
        }
        public void AddProduct(SANPHAM product)
        {
            // Thực hiện sao chép (clone) đối tượng sản phẩm
            var clonedProduct = CloneProduct(product);

            // Thêm sản phẩm vào danh sách và cơ sở dữ liệu
            products.Add(clonedProduct);
            database.SANPHAMs.Add(clonedProduct);
            database.SaveChanges();
        }

        private SANPHAM CloneProduct(SANPHAM originalProduct)
        {
            // Tạo một bản sao mới của sản phẩm
            var clonedProduct = new SANPHAM
            {
                MaSP = originalProduct.MaSP,
                TenSP = originalProduct.TenSP,
                Kichthuoc = originalProduct.Kichthuoc,
                Donvitinh = originalProduct.Donvitinh,
                Dongia = originalProduct.Dongia,
                Mota = originalProduct.Mota,
                Hinhminhhoa = originalProduct.Hinhminhhoa,
                MaLoaiSP = originalProduct.MaLoaiSP,
                Soluongban = originalProduct.Soluongban
            };
            return clonedProduct;
        }
    }
}