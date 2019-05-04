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
        public IEnumerable<Publication> publicationsById(int Id)
        {
            return (from t in cont.PublicationSet where t.Id == Id select t).OrderBy(book => book.Name);
        }
        public IEnumerable<Publication> publicationsByName(string Name)
        {
            return (from t in cont.PublicationSet where t.Name.Contains(Name) select t).OrderBy(book => book.Name);
        }
        public IEnumerable<Publication> publicationsByAuthor(string Name)
        {
            return (from t in cont.PublicationSet where t.Author.Contains(Name) select t).OrderBy(book => book.Name);
        }
        public IEnumerable<Publication> publicationsByBBK(string BBK)
        {
            return (from t in cont.PublicationSet where t.BBK == BBK select t).OrderBy(book => book.Name);
        }
        public IEnumerable<Publication> publicationsByPub(string Name)
        {
            return (from t in cont.PublicationSet where t.Publisher.Name.Contains(Name) select t).OrderBy(book => book.Name);
        }
        public IEnumerable<Publication> publicationsByAvail(bool Avail)
        {
            return (from t in cont.PublicationSet where t.Available==Avail select t).OrderBy(book => book.Name);
        }
        public void Add(Publication t, int Publishers)
        {
            Publisher pb = cont.PublisherSet.Find(Publishers);
            t.Publisher = pb;
            cont.PublicationSet.Add(t);
            cont.SaveChanges();
        }
    }
}