using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Practice.Models
{
    public class PenaltyDebtors
    {
        public string FIO;
        public string Bday;
        public string Phone;
        public string Email;
        public decimal Sum;
        public PenaltyDebtors(string _FIO, string _Bday, string _phone, string _Email, decimal _Sum)
        {
            FIO = _FIO;
            Bday = _Bday;
            Phone = _phone;
            Email = _Email;
            Sum = _Sum;
        }
    }
}