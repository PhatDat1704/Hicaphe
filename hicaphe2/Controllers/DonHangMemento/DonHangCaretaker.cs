using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hicaphe2.Controllers
{
    public class DonHangCaretaker
    {
        private Stack<DonHangMemento> history = new Stack<DonHangMemento>();

        public void Backup(DonHangMemento memento)
        {
            history.Push(memento);
        }

        public DonHangMemento Undo()
        {
            if (history.Count == 0)
                return null;

            return history.Pop();
        }

    }
}