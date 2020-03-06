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
    }
}
