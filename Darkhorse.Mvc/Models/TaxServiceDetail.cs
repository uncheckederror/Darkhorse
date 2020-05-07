using DarkHorse.DataAccess;
using System.Collections.Generic;

namespace DarkHorse.Mvc.Models
{
    public class TaxServiceDetail
    {
        public RealPropertyAccount Account { get; set; }
        public TaxService TaxService { get; set; }
    }
}
