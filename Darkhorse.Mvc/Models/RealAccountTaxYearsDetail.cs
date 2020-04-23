using DarkHorse.DataAccess;
using System.Collections.Generic;

namespace DarkHorse.Mvc.Models
{
    public class RealAccountTaxYearsDetail
    {
        public RealPropertyAccount Account { get; set; }
        public SeniorCitizenRate SeniorCitizen { get; set; }
        public RealPropertyAccountYear TaxYear { get; set; }
        public OtherAssessment OtherAssessment { get; set; }
        public IEnumerable<RealPropertyAccountYear> TaxYears { get; set; }
    }
}
