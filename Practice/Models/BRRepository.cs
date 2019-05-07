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
        public void Add(BookGiving BG, BookReturning BR)
        {
            try
            {
                if (BG != null && BR != null)
                {
                    Librarian curL = (from t in cont.PersonSet where BR.Librarian.Id == t.Id select t as Librarian).First();
                    BG.BookReturning = new BookReturning();
                    BG.BookReturning.Librarian = curL;
                    Penalty pn = new Penalty();
                    pn.Info = BR.Penalty.Info;
                    pn.Sum = BR.Penalty.Sum;
                    BG.BookReturning.Penalty = pn;
                    BG.BookReturning.Real_Return_Date = BR.Real_Return_Date;
                    BG.Publication.Available = true;
                    cont.SaveChanges();
                }
            }
            catch { }
        }
    }
}