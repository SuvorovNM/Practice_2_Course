using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Practice.Models
{
    public class PublisherRepository
    {
        private LibraryContainer1 cont;
        public PublisherRepository(LibraryContainer1 _cont)
        {
            cont = _cont;
        }
        public IEnumerable<Publisher> publishers()
        {
            return cont.PublisherSet.OrderBy(pub => pub.Name);
        }
        public void Add(Publisher p)
        {
            cont.PublisherSet.Add(p);
            cont.SaveChanges();
        }
        public Publisher GetPublisher(int id)
        {
            return cont.PublisherSet.Find(id);
        }
        public void Edit(int id, Publisher p)
        {
            Publisher t = GetPublisher(id);
            t.Name = p.Name;
            t.City = p.City;
            cont.SaveChanges();
        }
    }
}