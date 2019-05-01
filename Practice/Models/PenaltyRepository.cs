using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Practice.Models
{
    public class PenaltyRepository
    {
        private LibraryContainer1 cont;
        public PenaltyRepository(LibraryContainer1 _cont)
        {
            cont = _cont;
        }
        public IEnumerable<Penalty> penalties()
        {
            return cont.PenaltySet.OrderBy(pn => pn.Id);
        }
    }
}