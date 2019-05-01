using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Practice.Models
{
    public class BRRepository
    {
        private LibraryContainer1 cont;
        public BRRepository(LibraryContainer1 _cont)
        {
            cont = _cont;
        }
        public IEnumerable<BookReturning> bookReturnings()
        {
            return cont.BookReturningSet.OrderBy(br => br.Real_Return_Date);
        }
    }
}