using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Practice.Models
{
    public class BooksPopularity
    {
        public string Name;
        public string Author;
        public string Publisher_Name;
        public int Year;
        public int Count;
        public BooksPopularity(string _Name,string _Author, string _PName, int _Year, int _Count)
        {
            Name = _Name;
            Author = _Author;
            Publisher_Name = _PName;
            Year = _Year;
            Count = _Count;
        }
    }
}