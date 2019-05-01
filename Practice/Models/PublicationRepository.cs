using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Practice.Models
{
    public class PublicationRepository
    {
        private LibraryContainer1 cont;
        public PublicationRepository(LibraryContainer1 _cont)
        {
            cont = _cont;
        }
        public IEnumerable<Publication> publications()
        {
            return cont.PublicationSet.OrderBy(book => book.Name);
        }
    }
}