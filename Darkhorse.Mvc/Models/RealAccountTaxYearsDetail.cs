using DarkHorse.DataAccess;
using System.Collections.Generic;

namespace DarkHorse.Mvc.Models
{
    public class RealAccountTaxYearsDetail
    {
        public RealPropertyAccount Account { get; set; }
        public RealPropertyAccountsFilter AccountsFilter { get; set; }
        public SeniorCitizenRate SeniorCitizen { get; set; }
        public RealPropertyAccountYear TaxYear { get; set; }
        public IEnumerable<OtherAssessment> OtherAssessments { get; set; }
        public StormwaterManagement StormwaterManagement { get; set; }
        public FFPRate FFPRate { get; set; }
        public NoxiousWeedAssessment NoxiousWeedAssessment { get; set; }
        public RealPropertyYearPayment RealPropertyYearPayment { get; set; }
        public IEnumerable<RealPropertyPrepayment> RealPropertyPrepayments { get; set; }
        public IEnumerable<RealPropertyAdjustment> RealPropertyAdjustments { get; set; }
        public IEnumerable<RealPropertyExemptions> RealPropertyExemptions { get; set; }
        public IEnumerable<RealPropertyAccountYear> TaxYears { get; set; }
    }
}
