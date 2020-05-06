using DarkHorse.DataAccess;
using System.Collections.Generic;

namespace DarkHorse.Mvc.Models
{
    public class RealAccountDetail
    {
        public RealPropertyAccount Account { get; set; }
        public IEnumerable<Contact> Contacts { get; set; }
        public IEnumerable<LegalDescription> LegalDescriptions { get; set; }
        public IEnumerable<RealAccountParcel> Parcels { get; set; }
        public Plat Plat { get; set; }
        public IEnumerable<RealPropertySiteAddress> SiteAddresses { get; set; }
        public IEnumerable<NewConstructionDetail> Inspections { get; set; }
        public IEnumerable<RealPropertyAccountGroup> AccountGroups { get; set; }
        public IEnumerable<Notice> Notices { get; set; }
        public IEnumerable<SalesAccount> Sales { get; set; }
        public IEnumerable<AccountTag> Tags { get; set; }
        public IEnumerable<CrmContact> CrmContacts { get; set; }
        public IEnumerable<Building> Buildings { get; set; }
    }
}
