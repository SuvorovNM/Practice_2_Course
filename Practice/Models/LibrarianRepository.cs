using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Practice.Models
{
    public class LibrarianRepository
    {
        private LibraryContainer1 cont;
        public LibrarianRepository(LibraryContainer1 _cont)
        {
            cont = _cont;
        }
        public IEnumerable<Librarian> librarians()
        {
            return (from t in cont.PersonSet where (t as Librarian != null && !(t as Librarian).Deleted) select t as Librarian).OrderBy(lib => lib.FIO);
        }
        public Librarian LoginIn(int log, string pass)
        {
            int ex = (from t in cont.PersonSet where (t as Librarian != null && (t as Librarian).Staff_Number == log && (t as Librarian).Password == pass) select t as Librarian).Count();
            if (ex > 0)
                return (from t in cont.PersonSet where (t as Librarian != null && (t as Librarian).Staff_Number == log && (t as Librarian).Password == pass) select t as Librarian).First();
            else return null;
        }
    }
}