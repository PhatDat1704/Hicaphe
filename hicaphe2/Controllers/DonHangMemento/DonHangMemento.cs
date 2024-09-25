using hicaphe2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hicaphe2.Controllers
{
    public class DonHangMemento
    {
        public bool TrangThaiTruocKhiHuy { get; private set; }

        public DonHangMemento(bool trangThaiTruocKhiHuy)
        {
            TrangThaiTruocKhiHuy = trangThaiTruocKhiHuy;
        }
    }
}