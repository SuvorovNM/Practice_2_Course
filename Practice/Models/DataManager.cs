using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Practice.Models
{
    public class DataManager
    {
        private LibraryContainer1 cont;
        public BGRepository BG;
        public BRRepository BR;
        public LibrarianRepository Lib;
        public PenaltyRepository Penalty;
        public PublicationRepository Book;
        public PublisherRepository Pub;
        public ReaderRepository Rd;

        public DataManager()
        {
            cont = new LibraryContainer1();
            BG = new BGRepository(cont);
            BR = new BRRepository(cont);
            Lib = new LibrarianRepository(cont);
            Penalty = new PenaltyRepository(cont);
            Book = new PublicationRepository(cont);
            Pub = new PublisherRepository(cont);
            Rd = new ReaderRepository(cont);
        }
    }
}