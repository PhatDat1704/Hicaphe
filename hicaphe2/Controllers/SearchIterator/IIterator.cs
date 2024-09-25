using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hicaphe2.Controllers.SearchIterator
{
    internal interface IIterator<T>
    {
        bool HasNext();
        T Next();

    }
}
