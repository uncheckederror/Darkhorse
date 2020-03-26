using Darkhorse.DataAccess;
using System.Collections.Generic;

namespace Darkhorse.Mvc.Models
{
    public class RealAccountTaxYearsDetail
    {
        public RealPropertyAccount Account { get; set; }
        public IEnumerable<RealPropertyAccountYear> TaxYears { get; set; }
    }
}
