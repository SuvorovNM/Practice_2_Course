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
        public IEnumerable<BookGiving> bookGivings(Reader rd)
        {
            return (from t in cont.BookGivingSet where t.Reader.Id == rd.Id select t).OrderBy(bg => bg.Give_Date);
        }
        public void Add(BookGiving bg)
        {

            Reader curR = (from t in cont.PersonSet where bg.Reader.Id == t.Id select t as Reader).First();
            Librarian curL = (from t in cont.PersonSet where bg.Librarian.Id == t.Id select t as Librarian).First();
            Publication curP = (from t in cont.PublicationSet where t.Id == bg.Publication.Id select t).First();            
            bg.Reader = curR;
            bg.Librarian = curL;
            bg.Publication = curP;
            cont.BookGivingSet.Add(bg);
            curP.Available = false;
            cont.SaveChanges();
        }
        public BookGiving GetBookGiving(int id)
        {
            return cont.BookGivingSet.Find(id);
        }
        
    }
}