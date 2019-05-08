using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Practice.Models
{
    public class BooksByDate
    {
        public int LibTicket;
        public string Reader_FIO;
        public int Book_ID;
        public string Book_Name;
        public string Book_Author;
        public string LibFIO;
        public string ExpRetDate;
        public BooksByDate(int _LT, string _RFIO, int _BID, string _BN, string _BA, string _LFIO, string _ExpRetDate)
        {
            LibTicket = _LT;
            Reader_FIO = _RFIO;
            Book_ID = _BID;
            Book_Name = _BN;
            Book_Author = _BA;
            LibFIO = _LFIO;
            ExpRetDate = _ExpRetDate;
        }
    }
}