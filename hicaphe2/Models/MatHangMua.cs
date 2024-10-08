﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hicaphe2.Models
{
    public class MatHangMua
    {
        HiCaPheEntities2 db = new HiCaPheEntities2();
        private MatHangMua x;

        public string MaSP { get; set; }
        public string TenSP { get; set; }
        public string Hinhminhhoa { get; set; }
        public string Kichthuoc { get; set; }
        public double Dongia { get; set; }
        public int SoLuong { get; set; }



        public double ThanhTien()
        {
            return SoLuong * Dongia;
        }



        public MatHangMua(string MaSP)
        {
            this.MaSP = MaSP;
            var sanpham = db.SANPHAMs.Single(s => s.MaSP == this.MaSP);
            this.TenSP = sanpham.TenSP;
            this.Hinhminhhoa = sanpham.Hinhminhhoa;
            this.Kichthuoc=sanpham.Kichthuoc;
            this.Dongia = double.Parse(sanpham.Dongia.ToString());
            this.SoLuong = 1;
        }

        public MatHangMua(MatHangMua x)
        {
            this.x = x;
        }
    }
}