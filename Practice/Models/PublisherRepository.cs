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
    }
}