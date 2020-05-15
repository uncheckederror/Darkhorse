using DarkHorse.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkHorse.Mvc
{
    public class RealAccountNonprofitDetail
    {
        public NonprofitAccount SelectedAccount { get; set; }
        public IEnumerable<NonprofitAccount> AccountYears { get; set; }
    }
}
