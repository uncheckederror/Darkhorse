using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkHorse.Mvc.Models
{
    public class RealAccountSearchResult
    {
        public string AccountNumber { get; set; }
        public string AccountNumberSort { get; set; }
        public int ProcessNumber { get; set; }
        public string ActiveIndicatior { get; set; }
        public string Contact { get; set; }
        public string ContactSort { get; set; }
        public string ContactType { get; set; }
        public int StreetNumber { get; set; }
        public string StreetName { get; set; }
        public string StreetAddress { get; set; }
        public string StreetNameSort { get; set; }
        public string SectionTownshipRange { get; set; }
        public int AccountGroup { get; set; }
        public string QuarterSection { get; set; }
        public string Tags { get; set; }
        public string PropertyClass { get; set; }
        public decimal? ParcelAcreage { get; set; }
        public string TaxCode { get; set; }
    }
}