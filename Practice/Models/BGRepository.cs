using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Practice.Models
{
    public class BGRepository
    {
        private LibraryContainer1 cont;
        public BGRepository(LibraryContainer1 _cont)
        {
            cont = _cont;
        }
        public IEnumerable<BookGiving> bookGivings()
        {
            return cont.BookGivingSet.OrderBy(bg => bg.Give_Date);
        }
    }
}