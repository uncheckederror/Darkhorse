using DarkHorse.DataAccess;
using System.Collections.Generic;

namespace DarkHorse.Mvc.Models
{
    public class RealAccountTaxYearsDetail
    {
        public RealPropertyAccount Account { get; set; }
        public IEnumerable<RealPropertyAccountYear> TaxYears { get; set; }
    }
}