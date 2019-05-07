using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Practice.Models
{
    public class ReaderRepository
    {
        private LibraryContainer1 cont;
        public ReaderRepository(LibraryContainer1 _cont)
        {
            cont = _cont;
        }
        public IEnumerable<Reader> readers()
        {
            return (from t in cont.PersonSet where (t as Reader != null) select t as Reader).OrderBy(rd => rd.FIO);
        }
        public IEnumerable<Reader> readersByFIO(string FIO)
        {
            return (from t in cont.PersonSet where (t as Reader != null) && t.FIO==FIO select t as Reader).OrderBy(rd => rd.FIO);
        }
        public IEnumerable<Reader> readersByLibCard(int LC)
        {
            return (from t in cont.PersonSet where (t as Reader != null)&&(t as Reader).Library_Card==LC select t as Reader).OrderBy(rd => rd.FIO);
        }
        public IEnumerable<Reader> readersByBirthday(DateTime Birth)
        {
            return (from t in cont.PersonSet where (t as Reader != null)&&t.Birthday==Birth select t as Reader).OrderBy(rd => rd.FIO);
        }
        public IEnumerable<Reader> readersByPhone(string Phone)
        {
            return (from t in cont.PersonSet where (t as Reader != null)&&t.Phone_Number==Phone select t as Reader).OrderBy(rd => rd.FIO);
        }
        public IEnumerable<Reader> readersByRegDate(DateTime Date)
        {
            return (from t in cont.PersonSet where (t as Reader != null)&&(t as Reader).Registration_Date==Date select t as Reader).OrderBy(rd => rd.FIO);
        }
        public Reader GetReader(int _id)
        {
            return (Reader)cont.PersonSet.Find(_id);
        }
        public void Edit(int id, Reader t)
        {
            Reader rd = GetReader(id);
            if (rd != null)
            {
                rd.FIO = t.FIO;
                //rd.Birthday = t.Birthday;
                rd.Phone_Number = t.Phone_Number;
                rd.Email = t.Email;
                rd.Address.Region = t.Address.Region;
                rd.Address.City = t.Address.City;
                rd.Address.Street = t.Address.Street;
                rd.Address.House = t.Address.House;
                rd.Address.Flat = t.Address.Flat;
                cont.SaveChanges();
            }
        }
        public void Add(Reader r)
        {
            DateTime Today = DateTime.Today;
            r.Registration_Date = Today;
            int MaxLibTicket = (from t in cont.PersonSet where (t as Reader != null) select (t as Reader).Library_Card).Max();
            r.Library_Card = MaxLibTicket + 1;
            cont.PersonSet.Add(r);
            cont.SaveChanges();
        }
        public void DeleteReader(int id)
        {
            Reader t = GetReader(id);
            if (t != null)
            {
                try
                {
                    cont.PersonSet.Remove(t);
                    cont.SaveChanges();
                }
                catch { }
            }
        }
        public decimal GetPenalty(Reader r)
        {
            IEnumerable<decimal> dc = (from t in cont.BookReturningSet where t.BookGiving.Reader.Id == r.Id && t.Penalty != null && t.Penalty.Sum > 0 select t.Penalty.Sum);
            if (dc != null &&dc.Count()!=0)
                return dc.Sum();
            else return 0;
        }
    }
}