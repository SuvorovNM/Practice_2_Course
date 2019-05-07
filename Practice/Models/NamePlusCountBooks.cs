using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Practice.Models
{
    public class NamePlusCountBooks
    {
        public string FIO;
        public int Library_Card;
        public string Phone_Number;
        public string Email;        
        public string Registration_Date;
        public int Count;
        public NamePlusCountBooks(string _FIO, int _LC, string _PN, string _Email, DateTime _DT, int _Count)
        {
            FIO = _FIO;
            Library_Card = _LC;
            Phone_Number = _PN;
            Email = _Email;
            Registration_Date = _DT.ToShortDateString();
            Count = _Count;
        }
    }
}