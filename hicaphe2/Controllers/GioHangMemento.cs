using hicaphe2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hicaphe2.Controllers
{
    public class GioHangMemento
    {
        public List<MatHangMua> SanPhamTrongGio { get; private set; }

        public GioHangMemento(List<MatHangMua> items)
        {
            SanPhamTrongGio = items;
        }
    }


    public class GioHang
    {
        public List<MatHangMua> Items { get; set; }

        public GioHang()
        {
            Items = new List<MatHangMua>();
        }

        public GioHangMemento SaveMemento()
        {
            return new GioHangMemento(Items.Select(x => new MatHangMua(x)).ToList());
        }

        public void RestoreMemento(GioHangMemento memento)
        {
            if (memento != null)
            {
                Items = memento.SanPhamTrongGio;
            }
        }
    }


    public class GioHangCaretaker
    {
        private Stack<GioHangMemento> mementos = new Stack<GioHangMemento>();

        public void Backup(GioHang gioHang)
        {
            mementos.Push(gioHang.SaveMemento());
        }

        public void Undo(GioHang gioHang)
        {
            if (mementos.Count != 0)
            {
                GioHangMemento memento = mementos.Pop();
                gioHang.RestoreMemento(memento);
            }
        }
    }


    public class GioHangKhoPhuc : Controller
    {
        private GioHangCaretaker caretaker = new GioHangCaretaker();
        private GioHang gioHang = new GioHang();

        public ActionResult BackupGioHang()
        {
            caretaker.Backup(gioHang);
            return RedirectToAction("Index");
        }

        public ActionResult UndoGioHang()
        {
            caretaker.Undo(gioHang);
            return RedirectToAction("Index");
        }
    }
}