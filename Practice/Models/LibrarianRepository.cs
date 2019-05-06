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
        public Librarian GetLibrarian(int id)
        {
            return (from t in cont.PersonSet where (t as Librarian != null && t.Id == id) select t as Librarian).First();
        }
        public void Edit(int id, Librarian lib)
        {
            Librarian Old = GetLibrarian(id);
            Old.FIO = lib.FIO;
            Old.Birthday = lib.Birthday;
            Old.Phone_Number = lib.Phone_Number;
            Old.Email = lib.Email;
            Old.Hiring_Date = lib.Hiring_Date;
            Old.Password = lib.Password;
            Old.Address.Region = lib.Address.Region;
            Old.Address.City = lib.Address.City;
            Old.Address.Street = lib.Address.Street;
            Old.Address.House = lib.Address.House;
            Old.Address.Flat = lib.Address.Flat;
            cont.SaveChanges();
        }
        public void Add(Librarian lib)
        {
            lib.Hiring_Date = DateTime.Today;
            int Staff_Number = (from t in cont.PersonSet where (t as Librarian != null) select (t as Librarian).Staff_Number).Max() + 1;
            lib.Staff_Number = Staff_Number;
            cont.PersonSet.Add(lib);
            cont.SaveChanges();
        }
        public void Delete(int id)
        {
            Librarian lib = GetLibrarian(id);
            if (lib != null)
            {
                try
                {
                    lib.Deleted = true;
                    cont.SaveChanges();
                }
                catch { }
            }
        }
    }
}