using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hicaphe2.Controllers
{
    public class DonHangOriginator
    {
        private bool _trangThaiDonHang;

        public DonHangOriginator(bool trangThaiDonHang)
        {
            _trangThaiDonHang = trangThaiDonHang;
        }

        // Lưu trạng thái hiện tại vào Memento
        public DonHangMemento SaveToMemento()
        {
            return new DonHangMemento(_trangThaiDonHang);
        }

        // Khôi phục trạng thái từ Memento
        public void RestoreFromMemento(DonHangMemento memento)
        {
            _trangThaiDonHang = memento.TrangThaiTruocKhiHuy;
        }
    }
}