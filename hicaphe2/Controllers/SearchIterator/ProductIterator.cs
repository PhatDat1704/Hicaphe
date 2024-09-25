using hicaphe2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace hicaphe2.Controllers.SearchIterator
{
    public class ProductIterator:IIterator<SANPHAM>
    {
        private List<SANPHAM> sanPhams;
        private int position = 0;

        public ProductIterator(List<SANPHAM> sanPhams)
        {
            this.sanPhams = sanPhams;
        }

        public bool HasNext()
        {
            return position < sanPhams.Count;
        }

        public SANPHAM Next()
        {
            SANPHAM current = sanPhams[position];
            position++;
            return current;
        }
    }
}