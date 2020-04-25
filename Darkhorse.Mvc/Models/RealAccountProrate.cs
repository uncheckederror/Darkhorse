using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkHorse.Mvc.Models
{
    public class RealAccountProrate
    {
        public int RpAcctId { get; set; }
        public int RpAcctYrId { get; set; }
        public int TaxYear { get; set; }
        public DateTime StartDate { get; set; }
        public string Reason { get; set; }
    }
}
