using Darkhorse.DataAccess;
using System.Collections.Generic;

namespace Darkhorse.Mvc.Models
{
    public class RealAccountDetail
    {
        public RealPropertyAccount Account { get; set; }
        public IEnumerable<Contacts> Contacts { get; set; }
        public IEnumerable<LegalDiscription> LegalDiscriptions { get; set; }
        public Plat Plat { get; set; }
        public IEnumerable<RealPropertySiteAddress> SiteAddresses { get; set; }
        public IEnumerable<NewConstructionDetail> Inspections { get; set; }
        public IEnumerable<RealPropertyAccountGroup> AccountGroups { get; set; }
        public IEnumerable<Notice> Notices { get; set; }
    }
}
