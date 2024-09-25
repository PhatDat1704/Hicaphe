using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using hicaphe2.Models;

namespace hicaphe2.Controllers
{
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
}