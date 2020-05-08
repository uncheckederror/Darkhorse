using DarkHorse.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkHorse.Mvc
{
    public class RealAccountHistoryDetail
    {
        public RealPropertyAccount Account { get; set; }
        public RealPropertyAccountsFilter AccountsFilter { get; set; }
        public IEnumerable<RealAccountOwner> Owners { get; set; }
        public RealAccountOwner SelectedOwner { get; set; }
        public IEnumerable<Contact> Contacts { get; set; }
        public IEnumerable<RealPropertyAccountGroup> AccountGroups { get; set; }
        public IEnumerable<Notice> Notices { get; set; }
        public IEnumerable<AccountTag> AccountTags { get; set; }
    }
}
